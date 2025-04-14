using AltenTest.Domain.Entities;

namespace AltenTest.Domain.Repositories;

public interface IWishlistRepository
{
    Task<Wishlist?> GetByUserIdAsync(string userId);
    Task<Wishlist> AddAsync(Wishlist wishlist);
    Task<Wishlist> UpdateAsync(Wishlist wishlist);
} 