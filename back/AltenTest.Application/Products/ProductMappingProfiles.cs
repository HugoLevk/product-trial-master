using AutoMapper;
using AltenTest.Application.Products.DTOs;
using AltenTest.Domain.Entities;

namespace AltenTest.Application.Products;


public class ProductMappingProfiles : Profile
{
    public ProductMappingProfiles()
    {
        CreateMap<Product, ProductDTO>();
        CreateMap<CreateProductDTO, Product>();
        CreateMap<UpdateProductDTO, Product>();
    }
}

