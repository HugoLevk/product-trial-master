using AltenTest.Domain.Entities;
using AltenTest.Domain.Repositories;
using AltenTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AltenTest.Infrastructure.Repositories;

public class WishlistRepository(ApplicationDbContext context) : IWishlistRepository
{
    public async Task<Wishlist?> GetByUserIdAsync(string userId)
    {
        return await context.Wishlists
            .Include(w => w.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(w => w.UserId == userId);
    }

    public async Task<Wishlist> AddAsync(Wishlist wishlist)
    {
        context.Wishlists.Add(wishlist);
        await context.SaveChangesAsync();
        return wishlist;
    }

    public async Task<Wishlist> UpdateAsync(Wishlist wishlist)
    {
        context.Wishlists.Update(wishlist);
        await context.SaveChangesAsync();
        return wishlist;
    }
} 