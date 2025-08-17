using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_voting.Model;

[Index(nameof(VoterId), nameof(ElectionId), IsUnique = true)]

public class Vote
{
    [Key]
    public int VoteId { get; set; }

    public int VoterId { get; set; }
    [ForeignKey("VoterId")]
    public Voter Voter { get; set; }

    public int CandidateId { get; set; }
    [ForeignKey("CandidateId")]
    public Candidate Candidate { get; set; }

    public int ElectionId { get; set; }
    [ForeignKey("ElectionId")]
    public Election Election { get; set; }

    public DateTime VotedAt { get; set; } = DateTime.UtcNow;
}
