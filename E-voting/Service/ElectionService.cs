using E_voting.DTO;
using E_voting.Model;
using E_voting.Repository;
using E_voting.Responses;
using Microsoft.EntityFrameworkCore;

namespace E_voting.Service
{
    public class ElectionService : IElectionService
    {
        private readonly IElectionRepository _electionRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IVoteRepository _voteRepository;

        public ElectionService(IElectionRepository electionRepository , ICandidateRepository candidateRepository,IVoteRepository voteRepository)
        {
            _electionRepository = electionRepository;
            _candidateRepository = candidateRepository;
            _voteRepository = voteRepository;
        }


        public async Task<ApiResponse<List<Election>>> GetAllElectionsAsync()
        {
            try
            {
                var electionList = await _electionRepository
                    .GetAll()
                    .Where(e => e.IsActive).ToListAsync();


                if (electionList == null || electionList.Count == 0)
                    return new ApiResponse<List<Election>>(false, "No election active");

                return new ApiResponse<List<Election>>(true, "Successfully", electionList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Election>>(false, $"Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<Election>> GetElectionByIdAsync(int id)
        {
            try
            {
                var election = await _electionRepository.GetDetailsAsync(id);


                if (election == null || election.IsActive == false)
                    return new ApiResponse<Election>(false, "Election is inactive or not found");

                return new ApiResponse<Election>(true, "Election detail fetch successful", election);
            }
            catch (Exception ex)
            {
                return new ApiResponse<Election>(false, $"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<CandidateListDTO>>> GetCandidateListByElectionIdAsync(int id)
        {
            try
            {
                var election = await _electionRepository.GetDetailsAsync(id);
                if (election == null || !election.IsActive)
                    return new ApiResponse<List<CandidateListDTO>>(false, "Election is inactive or not found");
                
                var candidates = await _candidateRepository
                    .GetAll()
                    .Where(c => c.ElectionId == id)
                    .Select(c => new CandidateListDTO
                    {
                        CandidateId = c.CandidateId,
                        Name = c.Name,
                        Party = c.Party,
                        VoteCount = _voteRepository
                            .GetAll()
                            .Count(v => v.CandidateId == c.CandidateId && v.ElectionId == id)
                    }).ToListAsync();


                if (!candidates.Any())
                    return new ApiResponse<List<CandidateListDTO>>(false, "No candidates found");

                return new ApiResponse<List<CandidateListDTO>>(true, "Candidates fetched", candidates);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CandidateListDTO>>(false, $"Error: {ex.Message}");
            }
        }
    }
}
