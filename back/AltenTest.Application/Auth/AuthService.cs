using AltenTest.Application.Auth.DTOs;
using AltenTest.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AltenTest.Application.Auth;

public class AuthService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IConfiguration configuration) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IConfiguration _configuration = configuration;

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        User existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        User user = new User
        {
            UserName = registerDto.Username,
            Email = registerDto.Email,
            FirstName = registerDto.Firstname,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return new AuthResponseDto
        {
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        User user = await _userManager.FindByEmailAsync(loginDto.Email) ?? throw new InvalidOperationException("Invalid email or password");

        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isPasswordValid)
        {
            throw new InvalidOperationException("Invalid email or password");
        }

        user.UpdatedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        (string token, List<Claim> claims) = GenerateJwtTokenAndClaims(user);

        await SignInUserAsync(user, claims);

        return new AuthResponseDto
        {
            Token = token,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty
        };
    }

    private (string token, List<Claim> claims) GenerateJwtTokenAndClaims(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found in configuration")));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), claims);
    }

    private async Task SignInUserAsync(User user, List<Claim> claims)
    {
        bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        AuthenticationProperties authProperties = new AuthenticationProperties
        {
            IsPersistent = isDevelopment,
            ExpiresUtc = isDevelopment ? DateTimeOffset.UtcNow.AddHours(72) : DateTimeOffset.UtcNow.AddHours(3)
        };

        await _signInManager.SignInWithClaimsAsync(user, authProperties, claims);
    }
} 