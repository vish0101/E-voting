using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_voting.Model;

public class Candidate
{
    public int CandidateId { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; }

    [Required, MaxLength(100)]
    public string Party { get; set; }

    public int ElectionId { get; set; }

    [ForeignKey("ElectionId")]
    [JsonIgnore]
    public Election Election { get; set; }

    //public ICollection<Vote> Votes { get; set; }
}
