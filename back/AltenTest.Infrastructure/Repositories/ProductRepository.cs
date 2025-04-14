using AltenTest.Domain.Entities;
using AltenTest.Domain.Exceptions;
using AltenTest.Domain.Repositories;
using AltenTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AltenTest.Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await context.Products.ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        Product prod = await context.Products.FirstOrDefaultAsync(p => p.Id == id) ?? throw new NotFoundException(nameof(Product), id.ToString());
        return prod;
    }

    public async Task<Product> AddAsync(Product product)
    {
        product.CreatedAt = DateTime.Now;
        context.Add(product);
        await context.SaveChangesAsync();

        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        Product productToUpdate = await context.Products.FirstOrDefaultAsync(p => p.Id == product.Id) ?? throw new NotFoundException(nameof(Product), product.Id.ToString());
        productToUpdate.Code = product.Code;
        productToUpdate.Name = product.Name;
        productToUpdate.Description = product.Description;
        productToUpdate.Image = product.Image;
        productToUpdate.Category = product.Category;
        productToUpdate.Price = product.Price;
        productToUpdate.Quantity = product.Quantity;
        productToUpdate.InternalReference = product.InternalReference;
        productToUpdate.ShellId = product.ShellId;
        productToUpdate.InventoryStatus = product.InventoryStatus;
        productToUpdate.InternalReference = product.InternalReference;
        productToUpdate.Rating = product.Rating;
        productToUpdate.UpdatedAt = DateTime.Now;

        context.Update(productToUpdate);
        await context.SaveChangesAsync();

        return productToUpdate;
    }

    public async Task DeleteAsync(int id)
    {
        Product productToDelete = await context.Products.FirstOrDefaultAsync(p => p.Id == id) ?? throw new NotFoundException(nameof(Product), id.ToString());
        context.Remove(productToDelete);
        await context.SaveChangesAsync();
    }
}
