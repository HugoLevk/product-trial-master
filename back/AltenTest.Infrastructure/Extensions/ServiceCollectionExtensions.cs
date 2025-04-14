using AltenTest.Application.Products;
using AltenTest.Domain.Entities;
using AltenTest.Domain.Interfaces;
using AltenTest.Domain.Repositories;
using AltenTest.Infrastructure.Authorization.Services;
using AltenTest.Infrastructure.Persistence;
using AltenTest.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AltenTest.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            5,
                            TimeSpan.FromSeconds(30),
                            null);
                        sqlOptions.CommandTimeout(30);
                    }));

        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IWishlistRepository, WishlistRepository>();

        services.AddScoped<IProductAuthorizationService, ProductAuthorizationService>();
    }
}
