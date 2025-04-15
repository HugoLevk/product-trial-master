
using AltenTest.Domain.Entities;
using AltenTest.Domain.Repositories;
using AltenTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AltenTest.Infrastructure.Repositories;

public class CartRepository(ApplicationDbContext context) : ICartRepository
{
    public async Task<Cart?> GetByUserIdAsync(string userId)
    {
        return await context.Carts
        .Include(c => c.Items)
            .ThenInclude(i => i.Product)
        .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Cart> AddAsync(Cart cart)
    {
        await context.Carts.AddAsync(cart);
        await context.SaveChangesAsync();
        return cart;
    }

    public async Task DeleteAsync(Cart cart)
    {
        context.Carts.Remove(cart);
        await context.SaveChangesAsync();
    }

    public async Task<Cart> UpdateAsync(Cart cart)
    {
        context.Carts.Update(cart);
        await context.SaveChangesAsync();
        return cart;    
    }
}

