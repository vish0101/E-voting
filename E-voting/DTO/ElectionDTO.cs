using System.ComponentModel.DataAnnotations;

namespace E_voting.DTO
{
    public class ElectionDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
