using AltenTest.Domain.Entities;

namespace AltenTest.Domain.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(string userId);
    Task<Cart> AddAsync(Cart cart);
    Task<Cart> UpdateAsync(Cart cart);
} 