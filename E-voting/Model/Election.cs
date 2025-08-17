using System.ComponentModel.DataAnnotations;

namespace E_voting.Model;

public class Election
{
    public int ElectionId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    public ICollection<Candidate> Candidates { get; set; }

    public ICollection<Vote> Votes { get; set; }
}
