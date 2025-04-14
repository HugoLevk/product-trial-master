using AltenTest.Application.Carts.DTOs;
using AltenTest.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AltenTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Identity.Application")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        CartDto cart = await _cartService.GetUserCartAsync();
        return Ok(cart);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDto addToCartDto)
    {
        CartDto cart = await _cartService.AddToCartAsync(addToCartDto);
        return Ok(cart);
    }

    [HttpPut("items")]
    public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto updateCartItemDto)
    {
        CartDto cart = await _cartService.UpdateCartItemAsync(updateCartItemDto);
        return Ok(cart);
    }

    [HttpDelete("items/{cartId}/{productId}")]
    public async Task<IActionResult> RemoveFromCart(int cartId, int productId)
    {
        await _cartService.RemoveFromCartAsync(cartId, productId);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        await _cartService.ClearCartAsync();
        return NoContent();
    }
}