using AltenTest.Application.Carts;
using AltenTest.Application.Interfaces.Services;
using AltenTest.Application.Products;
using AltenTest.Application.Wishlists;
using Microsoft.Extensions.DependencyInjection;

namespace AltenTest.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        System.Reflection.Assembly applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddAutoMapper(applicationAssembly);

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IWishlistService, WishlistService>();
    }
}
