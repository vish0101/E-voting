using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace E_voting.Model;

[Index(nameof(Email), IsUnique = true)]
public class Voter
{
    public int VoterId { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; }

    [Required, MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
    public string AadharNumber { get; set; }

    public ICollection<Vote> Votes { get; set; }

    public string role { get; set; } = "Voter";
}
