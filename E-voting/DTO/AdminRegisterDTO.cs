using System.ComponentModel.DataAnnotations;

namespace E_voting.DTO
{
    public class AdminRegister
    {
        [Required]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(16),MinLength(8)]
        public string Password { get; set; }
    }
}
