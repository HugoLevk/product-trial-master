using AltenTest.Application.Products.DTOs;

namespace AltenTest.Application.Products;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetAllAsync();
    Task<ProductDTO> GetByIdAsync(int id);
    Task<ProductDTO> AddAsync(CreateProductDTO createProductDTO);
    Task<ProductDTO> UpdateAsync(UpdateProductDTO updateProductDTO);
    Task DeleteAsync(int id);
}