using AltenTest.Application.Products.DTOs;
using AltenTest.Domain.Entities;
using AltenTest.Domain.Enums;
using AltenTest.Domain.Exceptions;
using AltenTest.Domain.Interfaces;
using AltenTest.Domain.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AltenTest.Application.Products;


public class ProductService(
    IMapper mapper,
    IProductRepository productRepository,
    IProductAuthorizationService authorizationService,
    IHttpContextAccessor httpContextAccessor) : IProductService
{
    private string GetCurrentUserEmail()
    {
        ClaimsPrincipal? user = (httpContextAccessor.HttpContext?.User) ?? throw new UnauthorizedAccessException("User not authenticated");
        Claim? emailClaim = user.FindFirst(ClaimTypes.Email) ?? throw new UnauthorizedAccessException("User email not found in claims");
        return emailClaim.Value;
    }

    public async Task<IEnumerable<ProductDTO>> GetAllAsync()
    {
        string userEmail = GetCurrentUserEmail();
        IEnumerable<Product> products = await productRepository.GetAllAsync();

        foreach (Product product in products)
        {
            if (!authorizationService.Authorize(product, ResourceOperation.Read, userEmail))
            {
                throw new UnauthorizedAccessException($"You are not authorized to read product {product.Name}");
            }
        }

        return mapper.Map<IEnumerable<ProductDTO>>(products);
    }

    public async Task<ProductDTO> GetByIdAsync(int id)
    {
        string userEmail = GetCurrentUserEmail();
        Product? product = await productRepository.GetByIdAsync(id) ?? throw new NotFoundException("Product", id.ToString());

        if (!authorizationService.Authorize(product, ResourceOperation.Read, userEmail))
        {
            throw new UnauthorizedAccessException($"You are not authorized to read product {product.Name}");
        }

        return mapper.Map<ProductDTO>(product);
    }

    public async Task<ProductDTO> AddAsync(CreateProductDTO createProductDTO)
    {
        string userEmail = GetCurrentUserEmail();
        Product product = mapper.Map<Product>(createProductDTO);

        if (!authorizationService.Authorize(product, ResourceOperation.Create, userEmail))
        {
            throw new UnauthorizedAccessException("You are not authorized to create products");
        }

        Product createdProduct = await productRepository.AddAsync(product);
        return mapper.Map<ProductDTO>(createdProduct);
    }

    public async Task<ProductDTO> UpdateAsync(UpdateProductDTO updateProductDTO)
    {
        string userEmail = GetCurrentUserEmail();
        Product product = mapper.Map<Product>(updateProductDTO);

        Product? existingProduct = await productRepository.GetByIdAsync(product.Id) ?? throw new NotFoundException("Product", product.Id.ToString());

        if (!authorizationService.Authorize(existingProduct, ResourceOperation.Update, userEmail))
        {
            throw new UnauthorizedAccessException($"You are not authorized to update product {existingProduct.Name}");
        }

        Product updatedProduct = await productRepository.UpdateAsync(product);
        return mapper.Map<ProductDTO>(updatedProduct);
    }

    public async Task DeleteAsync(int id)
    {
        string userEmail = GetCurrentUserEmail();
        Product? product = await productRepository.GetByIdAsync(id) ?? throw new NotFoundException("Product", id.ToString());

        if (!authorizationService.Authorize(product, ResourceOperation.Delete, userEmail))
        {
            throw new UnauthorizedAccessException($"You are not authorized to delete product {product.Name}");
        }

        await productRepository.DeleteAsync(id);
    }
}
