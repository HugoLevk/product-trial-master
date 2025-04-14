using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AltenTest.Domain.Entities;

public class User : IdentityUser
{
    [MaxLength(255)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(255)]
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
