using AltenTest.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AltenTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Identity.Application")]
public class WishlistController : ControllerBase
{
    private readonly IWishlistService _wishlistService;

    public WishlistController(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    [HttpGet]
    public async Task<IActionResult> GetWishlist()
    {
        Application.Wishlists.DTOs.WishlistDto wishlist = await _wishlistService.GetUserWishlistAsync();
        return Ok(wishlist);
    }

    [HttpPost("items/{productId}")]
    public async Task<IActionResult> AddToWishlist(int productId)
    {
        Application.Wishlists.DTOs.WishlistDto wishlist = await _wishlistService.AddToWishlistAsync(productId);
        return Ok(wishlist);
    }

    [HttpDelete("items/{wishlistId}/{productId}")]
    public async Task<IActionResult> RemoveFromWishlist(int wishlistId, int productId)
    {
        await _wishlistService.RemoveFromWishlistAsync(wishlistId, productId);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> ClearWishlist()
    {
        await _wishlistService.ClearWishlistAsync();
        return NoContent();
    }
}