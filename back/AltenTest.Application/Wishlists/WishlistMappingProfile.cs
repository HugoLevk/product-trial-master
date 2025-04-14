using AltenTest.Application.Wishlists.DTOs;
using AltenTest.Domain.Entities;
using AutoMapper;

namespace AltenTest.Application.Wishlists;

public class WishlistMappingProfile : Profile
{
    public WishlistMappingProfile()
    {
        CreateMap<Wishlist, WishlistDto>();
        CreateMap<WishlistItem, WishlistItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price));
    }
} 