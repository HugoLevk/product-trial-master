using AltenTest.Domain.Entities;

namespace AltenTest.Application.Carts.DTOs;

public class CartDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}