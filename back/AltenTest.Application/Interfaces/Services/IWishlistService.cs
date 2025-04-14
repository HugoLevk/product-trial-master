using AltenTest.Application.Wishlists.DTOs;

namespace AltenTest.Application.Interfaces.Services;

public interface IWishlistService
{
    Task<WishlistDto> GetUserWishlistAsync();
    Task<WishlistDto> AddToWishlistAsync(int productId);
    Task RemoveFromWishlistAsync(int wishlistId, int productId);
    Task ClearWishlistAsync();
} 