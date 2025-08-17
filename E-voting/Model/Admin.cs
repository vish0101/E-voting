using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace E_voting.Model;

[Index(nameof(Email), IsUnique = true)]

public class Admin
{
    public int AdminId { get; set; }

    public string Name {  get; set; }

    [Required, MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public string Role { get; set; } = "Admin";
}
