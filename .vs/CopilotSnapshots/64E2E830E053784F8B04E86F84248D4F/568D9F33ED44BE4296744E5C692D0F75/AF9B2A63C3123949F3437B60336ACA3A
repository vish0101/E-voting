using E_voting.DTO;
using E_voting.JWT;
using E_voting.Model;
using E_voting.Repository;
using E_voting.Responses;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace E_voting.Service
{
    public class VoterService : IVoterService
    {
        private readonly IVoterRepository _voterRepository;
        private readonly TokenGenerator _tokenGenerator;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IVoteRepository _voteRepository;

        public VoterService(IVoterRepository voterRepository, TokenGenerator tokenGenerator, ICandidateRepository candidateRepository, IVoteRepository voteRepository)
        {
            _voterRepository = voterRepository;
            _tokenGenerator = tokenGenerator;
            _candidateRepository = candidateRepository;
            _voteRepository = voteRepository;
        }

        public async Task<ApiResponse<VoterRegisterDTO>> RegisterVoterAsync(VoterRegisterDTO dto)
        {
            try
            {
                var validationContext = new ValidationContext(dto);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(dto, validationContext, validationResults, true))
                {
                    return new ApiResponse<VoterRegisterDTO>(false, string.Join("; ", validationResults.Select(r => r.ErrorMessage)));
                }

                var existingVoter = await _voterRepository
                    .GetAll()
                    .FirstOrDefaultAsync(v => v.Email == dto.Email || v.AadharNumber == dto.AadharNumber);

                if (existingVoter != null)
                {
                    return new ApiResponse<VoterRegisterDTO>(false, "Voter with this Email or Aadhaar number already exists.", null);
                }

                var voter = new Voter
                {
                    FullName = dto.FullName,
                    AadharNumber = dto.AadharNumber,
                    Email = dto.Email,
                    Password = dto.Password,
                };

                int result = await _voterRepository.InsertAsync(voter);
                if (result != 0)
                {
                    return new ApiResponse<VoterRegisterDTO>(true, "Voter registered successfully", dto);
                }
                else
                {
                    return new ApiResponse<VoterRegisterDTO>(false, "Voter not registered");
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<VoterRegisterDTO>(false, $"Error: {ex.Message}");
            }
        }

        public async Task<string?> LoginVoterAsync(VoterLoginDTO loginDto)
        {
            try
            {
                var validationContext = new ValidationContext(loginDto);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(loginDto, validationContext, validationResults, true))
                {
                    return null;
                }

                var voter = await _voterRepository
                    .GetAll()
                    .FirstOrDefaultAsync(a => a.Email == loginDto.Email && a.Password == loginDto.Password);
                if (voter == null) return null;

                return _tokenGenerator.GenerateToken(voter.VoterId.ToString(), voter.Email, voter.role);
            }
            catch
            {
                return null;
            }
        }

        public Task<IEnumerable<Vote>> GetAllVoterAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Admin> GetVoterByIdAsync(int id)
        {
            var voter = await _voterRepository.GetAll().FirstOrDefaultAsync(v => v.VoterId == id);
            if (voter == null) return null;
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<bool>> DeleteVoterAsync(int id)
        {
            try
            {
                var voter = await _voterRepository.GetAll().FirstOrDefaultAsync(v => v.VoterId == id);
                if (voter == null)
                    return new ApiResponse<bool>(false, "No voter found with this id");

                var result = await _voterRepository.DeleteAsync(id);
                if (result == 0)
                    return new ApiResponse<bool>(false, "Delete unsuccessful!!");
                else
                    return new ApiResponse<bool>(true, $"Voter with id {id} deleted successfully", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<string>> CastVoteAsync(int voterId, CastVoteDTO voteDTO)
        {
            try
            {
                var validationContext = new ValidationContext(voteDTO);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(voteDTO, validationContext, validationResults, true))
                {
                    return new ApiResponse<string>(false, string.Join("; ", validationResults.Select(r => r.ErrorMessage)));
                }

                var voter = await _voterRepository.GetAll().FirstOrDefaultAsync(v => v.VoterId == voterId);
                if (voter == null)
                    return new ApiResponse<string>(false, "No voter found with this id");

                var candidate = await _candidateRepository.GetAll()
                    .FirstOrDefaultAsync(c => c.CandidateId == voteDTO.CandidateId && c.ElectionId == voteDTO.ElectionId);

                if (candidate == null)
                    return new ApiResponse<string>(false, "Invalid candidate or election.");

                var alreadyVoted = await _voteRepository.GetAll()
                    .AnyAsync(v => v.VoterId == voterId && v.Candidate.ElectionId == voteDTO.ElectionId);

                if (alreadyVoted)
                    return new ApiResponse<string>(false, "Voter has already cast a vote in this election.");

                var vote = new Vote
                {
                    VoterId = voterId,
                    CandidateId = voteDTO.CandidateId,
                    ElectionId = voteDTO.ElectionId,
                    VotedAt = DateTime.UtcNow
                };

                var result = await _voteRepository.InsertAsync(vote);

                if (result > 0)
                    return new ApiResponse<string>(true, "Vote cast successfully.");

                return new ApiResponse<string>(false, "Failed to cast vote.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(false, $"Error: {ex.Message}");
            }
        }
    }
}
