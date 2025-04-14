namespace AltenTest.Domain.Entities;

public class Wishlist
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; } = null!;
    public List<WishlistItem> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 