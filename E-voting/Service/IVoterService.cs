using E_voting.DTO;
using E_voting.Model;
using E_voting.Responses;

namespace E_voting.Service
{
    public interface IVoterService
    {
        Task<ApiResponse<VoterRegisterDTO>> RegisterVoterAsync(VoterRegisterDTO VoterDto);
        Task<string?> LoginVoterAsync(VoterLoginDTO loginDto);

        Task<IEnumerable<Vote>> GetAllVoterAsync();
        Task<Admin> GetVoterByIdAsync(int id);
        Task<ApiResponse<bool>> DeleteVoterAsync(int id);

        Task<ApiResponse<string>> CastVoteAsync(int voterId , CastVoteDTO voteDTO);
    }
}
