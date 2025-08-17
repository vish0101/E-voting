using E_voting.DTO;
using E_voting.Model;
using E_voting.Responses;

namespace E_voting.Service
{
    public interface IElectionService
    {
        Task<ApiResponse<List<Election>>> GetAllElectionsAsync();
        Task<ApiResponse<Election>> GetElectionByIdAsync(int id);
        Task<ApiResponse<List<CandidateListDTO>>> GetCandidateListByElectionIdAsync(int id);

       // Task<Election> CreateElectionAsync(ElectionDTO dto);
        //Task<bool> UpdateElectionAsync(int id, ElectionDTO dto);
       // Task<bool> DeleteElectionAsync(int id);
    }
}
