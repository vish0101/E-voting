using E_voting.Model;
using E_voting.Repository;
using E_voting.Responses;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace E_voting.Service;

public class CandidateService : ICandidateService
{
    private readonly ICandidateRepository _candidateRepository;
    public CandidateService(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<ApiResponse<Candidate>> AddCandidateAsync(Candidate candidate)
    {
        try
        {
            var validationContext = new ValidationContext(candidate);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(candidate, validationContext, validationResults, true))
            {
                return new ApiResponse<Candidate>(false, string.Join("; ", validationResults.Select(r => r.ErrorMessage)));
            }

            var existingCandidate = await _candidateRepository
                .GetAll()
                .FirstOrDefaultAsync(c =>
                    c.ElectionId == candidate.ElectionId &&
                    c.Name.ToLower() == candidate.Name.ToLower() &&
                    c.Party.ToLower() == candidate.Party.ToLower());
            if (existingCandidate != null)
            {
                return new ApiResponse<Candidate>(false, "Candidate with the same name and party already exists in this election.");
            }

            var result = await _candidateRepository.InsertAsync(candidate);
            if (result == 0)
                return new ApiResponse<Candidate>(false, "Candidate Added Unsuccessfully");

            return new ApiResponse<Candidate>(true, "Candidate added successfully", candidate);
        }
        catch (Exception ex)
        {
            return new ApiResponse<Candidate>(false, $"Error: {ex.Message}");
        }
    }
}
