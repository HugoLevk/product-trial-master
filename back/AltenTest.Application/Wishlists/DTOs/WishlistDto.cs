namespace AltenTest.Application.Wishlists.DTOs;

public class WishlistDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public List<WishlistItemDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 