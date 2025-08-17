using System.ComponentModel.DataAnnotations;

namespace E_voting.DTO
{
    public class AdminLoginDTO
    {
        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(8), MaxLength(16)]
        public string Password { get; set; }
    }
}
