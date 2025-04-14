using AltenTest.Application.Interfaces.Services;
using AltenTest.Application.Wishlists.DTOs;
using AltenTest.Domain.Entities;
using AltenTest.Domain.Exceptions;
using AltenTest.Domain.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AltenTest.Application.Wishlists;

public class WishlistService : IWishlistService
{
    private readonly IMapper _mapper;
    private readonly IWishlistRepository _wishlistRepository;
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WishlistService(
        IMapper mapper,
        IWishlistRepository wishlistRepository,
        IProductRepository productRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _wishlistRepository = wishlistRepository;
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetCurrentUserId()
    {
        ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User ?? 
            throw new UnauthorizedAccessException("User not authenticated");
        Claim? userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier) ?? 
            throw new UnauthorizedAccessException("User ID not found in claims");
        return userIdClaim.Value;
    }

    public async Task<WishlistDto> GetUserWishlistAsync()
    {
        string userId = GetCurrentUserId();
        Wishlist? wishlist = await _wishlistRepository.GetByUserIdAsync(userId);
        
        if (wishlist == null)
        {
            wishlist = new Wishlist
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            wishlist = await _wishlistRepository.AddAsync(wishlist);
        }

        return _mapper.Map<WishlistDto>(wishlist);
    }

    public async Task<WishlistDto> AddToWishlistAsync(int productId)
    {
        string userId = GetCurrentUserId();
        Wishlist? wishlist = await _wishlistRepository.GetByUserIdAsync(userId);
        
        if (wishlist == null)
        {
            wishlist = new Wishlist
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            wishlist = await _wishlistRepository.AddAsync(wishlist);
        }

        Product? product = await _productRepository.GetByIdAsync(productId) ?? 
            throw new NotFoundException("Product", productId.ToString());

        if (!wishlist.Items.Any(i => i.ProductId == productId))
        {
            wishlist.Items.Add(new WishlistItem
            {
                WishlistId = wishlist.Id,
                ProductId = product.Id,
                CreatedAt = DateTime.UtcNow
            });
        }

        wishlist.UpdatedAt = DateTime.UtcNow;
        await _wishlistRepository.UpdateAsync(wishlist);

        return _mapper.Map<WishlistDto>(wishlist);
    }

    public async Task RemoveFromWishlistAsync(int wishlistId, int productId)
    {
        string userId = GetCurrentUserId();
        Wishlist? wishlist = await _wishlistRepository.GetByUserIdAsync(userId) ?? 
            throw new NotFoundException("Wishlist", "User wishlist not found");

        WishlistItem? wishlistItem = wishlist.Items.FirstOrDefault(i => 
            i.WishlistId == wishlistId && 
            i.ProductId == productId) ?? 
            throw new NotFoundException("WishlistItem", $"WishlistItem not found for Wishlist {wishlistId} and Product {productId}");

        wishlist.Items.Remove(wishlistItem);
        wishlist.UpdatedAt = DateTime.UtcNow;

        await _wishlistRepository.UpdateAsync(wishlist);
    }

    public async Task ClearWishlistAsync()
    {
        string userId = GetCurrentUserId();
        Wishlist? wishlist = await _wishlistRepository.GetByUserIdAsync(userId) ?? 
            throw new NotFoundException("Wishlist", "User wishlist not found");

        wishlist.Items.Clear();
        wishlist.UpdatedAt = DateTime.UtcNow;

        await _wishlistRepository.UpdateAsync(wishlist);
    }
} 