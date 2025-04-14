using AltenTest.API.MiddleWares;
using AltenTest.Application.Extensions;
using AltenTest.Application.Auth;
using AltenTest.Domain.Entities;
using AltenTest.Infrastructure.Extensions;
using AltenTest.Infrastructure.Persistence;
using AltenTest.Infrastructure.Persistence.Seeding;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

WebApplication app = builder.Build();

ConfigureApp(app);

// Migrate and Seed Data
using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;
    try
    {
        ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        UserManager<User> userManager = services.GetRequiredService<UserManager<User>>();
        await UserSeeder.SeedUsersAsync(userManager);
        await ProductSeeder.SeedProductsAsync(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

await app.RunAsync();

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddScoped<ErrorHandlingMiddleware>();

    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    builder.Services.AddScoped<IAuthService, AuthService>();

    ConfigureIdentityAndAuth(builder);

    ConfigureSwagger(builder);
}

static void ConfigureIdentityAndAuth(WebApplicationBuilder builder)
{
    builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        // Password settings
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;

        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // User settings
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.None
            : CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = builder.Environment.IsDevelopment()
            ? TimeSpan.FromHours(3)
            : TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.LoginPath = "/token";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/access-denied";
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
        };
    });
}

static void ConfigureSwagger(WebApplicationBuilder builder)
{
    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "AltenTest API",
                Version = "v1",
                Description = "API documentation for AltenTest"
            });
            
            c.SwaggerGeneratorOptions.Servers = new List<OpenApiServer>
            {
                new OpenApiServer { Url = "https://localhost:5001", Description = "HTTPS Server" }
            };
        });
    }
}

static void ConfigureApp(WebApplication app)
{

    app.UseMiddleware<ErrorHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "AltenTest API v1");
            c.RoutePrefix = string.Empty;
            c.ConfigObject.AdditionalItems["schemes"] = new[] { "https" };
        });
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}
