using AltenTest.Application.Carts.DTOs;

namespace AltenTest.Application.Interfaces.Services;

public interface ICartService
{
    Task<CartDto> GetUserCartAsync();
    Task<CartDto> AddToCartAsync(AddToCartDto addToCartDto);
    Task<CartDto> UpdateCartItemAsync(UpdateCartItemDto updateCartItemDto);
    Task RemoveFromCartAsync(int cartId, int productId);
    Task ClearCartAsync();
} 