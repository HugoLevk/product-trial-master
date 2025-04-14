using AltenTest.Application.Carts.DTOs;
using AltenTest.Application.Interfaces.Services;
using AltenTest.Domain.Entities;
using AltenTest.Domain.Exceptions;
using AltenTest.Domain.Interfaces;
using AltenTest.Domain.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AltenTest.Application.Carts;

public class CartService : ICartService
{
    private readonly IMapper _mapper;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(
        IMapper mapper,
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetCurrentUserId()
    {
        ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User ?? 
            throw new UnauthorizedAccessException("User not authenticated");
        Claim? userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier) ?? 
            throw new UnauthorizedAccessException("User ID not found in claims");
        return userIdClaim.Value;
    }

    public async Task<CartDto> GetUserCartAsync()
    {
        string userId = GetCurrentUserId();
        Cart? cart = await _cartRepository.GetByUserIdAsync(userId);
        
        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            cart = await _cartRepository.AddAsync(cart);
        }

        return _mapper.Map<CartDto>(cart);
    }

    public async Task<CartDto> AddToCartAsync(AddToCartDto addToCartDto)
    {
        string userId = GetCurrentUserId();
        Cart? cart = await _cartRepository.GetByUserIdAsync(userId);
        
        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            cart = await _cartRepository.AddAsync(cart);
        }

        Product? product = await _productRepository.GetByIdAsync(addToCartDto.ProductId) ?? 
            throw new NotFoundException("Product", addToCartDto.ProductId.ToString());

        CartItem? existingItem = cart.Items.FirstOrDefault(i => i.ProductId == addToCartDto.ProductId);
        
        if (existingItem != null)
        {
            existingItem.Quantity += addToCartDto.Quantity;
            existingItem.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                CartId = cart.Id,
                ProductId = product.Id,
                Quantity = addToCartDto.Quantity,
                CreatedAt = DateTime.UtcNow
            });
        }

        cart.UpdatedAt = DateTime.UtcNow;
        await _cartRepository.UpdateAsync(cart);

        return _mapper.Map<CartDto>(cart);
    }

    public async Task<CartDto> UpdateCartItemAsync(UpdateCartItemDto updateCartItemDto)
    {
        string userId = GetCurrentUserId();
        Cart? cart = await _cartRepository.GetByUserIdAsync(userId) ?? 
            throw new NotFoundException("Cart", "User cart not found");

        CartItem? cartItem = cart.Items.FirstOrDefault(i => 
            i.CartId == updateCartItemDto.CartId && 
            i.ProductId == updateCartItemDto.ProductId) ?? 
            throw new NotFoundException("CartItem", $"CartItem not found for Cart {updateCartItemDto.CartId} and Product {updateCartItemDto.ProductId}");

        cartItem.Quantity = updateCartItemDto.Quantity;
        cartItem.UpdatedAt = DateTime.UtcNow;
        cart.UpdatedAt = DateTime.UtcNow;

        await _cartRepository.UpdateAsync(cart);
        return _mapper.Map<CartDto>(cart);
    }

    public async Task RemoveFromCartAsync(int cartId, int productId)
    {
        string userId = GetCurrentUserId();
        Cart? cart = await _cartRepository.GetByUserIdAsync(userId) ?? 
            throw new NotFoundException("Cart", "User cart not found");

        CartItem? cartItem = cart.Items.FirstOrDefault(i => 
            i.CartId == cartId && 
            i.ProductId == productId) ?? 
            throw new NotFoundException("CartItem", $"CartItem not found for Cart {cartId} and Product {productId}");

        cart.Items.Remove(cartItem);
        cart.UpdatedAt = DateTime.UtcNow;

        await _cartRepository.UpdateAsync(cart);
    }

    public async Task ClearCartAsync()
    {
        string userId = GetCurrentUserId();
        Cart? cart = await _cartRepository.GetByUserIdAsync(userId) ?? 
            throw new NotFoundException("Cart", "User cart not found");

        cart.Items.Clear();
        cart.UpdatedAt = DateTime.UtcNow;

        await _cartRepository.UpdateAsync(cart);
    }
} 