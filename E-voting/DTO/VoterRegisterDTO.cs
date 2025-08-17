using System.ComponentModel.DataAnnotations;

namespace E_voting.DTO
{
    public class VoterRegisterDTO
    {
        [Required , MaxLength(30)]
        public string FullName { get; set; }

        [EmailAddress ,MaxLength(100)]
        public string Email { get; set; }
        [Required , MaxLength(16),MinLength(8)]
        public string Password { get; set; }
        [Required]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhar number must be exactly 12 digits.")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Aadhar number must be numeric and exactly 12 digits.")]

        public string AadharNumber { get; set; }
    }
}
