
namespace AltenTest.Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; } = null!;
    public List<CartItem> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

