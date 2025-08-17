using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_voting.DTO;
using E_voting.JWT;
using E_voting.Model;
using E_voting.Repository;
using E_voting.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace E_voting.Service;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;
    private readonly TokenGenerator _tokenGenerator;
    private IElectionRepository _electionRepository;
    private ICandidateRepository _candidateRepository;

    public AdminService(IAdminRepository adminRepo, TokenGenerator tokenGenerator, IElectionRepository electionRepository, ICandidateRepository candidateRepository)
    {
        _adminRepository = adminRepo;
        _tokenGenerator = tokenGenerator;
        _electionRepository = electionRepository;
        _candidateRepository = candidateRepository;
    }

    public async Task<ApiResponse<AdminRegister>> RegisterAdminAsync(AdminRegister adminDto)
    {
        try
        {
            var existingAdmin = await _adminRepository
                .GetAll()
                .FirstOrDefaultAsync(a => a.Email == adminDto.Email);

            if (existingAdmin != null)
            {
                return new ApiResponse<AdminRegister>(false, "Email is already registered", null);
            }

            Admin admin = new Admin
            {
                Name = adminDto.Name,
                Email = adminDto.Email,
                Password = adminDto.Password
            };

            int result = await _adminRepository.InsertAsync(admin);

            if (result > 0)
            {
                return new ApiResponse<AdminRegister>(true, "Admin registered successfully", adminDto);
            }
            else
            {
                return new ApiResponse<AdminRegister>(false, "Failed to register admin");
            }
        }
        catch (Exception ex)
        {
            return new ApiResponse<AdminRegister>(false, $"Error: {ex.Message}");
        }
    }


    public async Task<string?> LoginAdminAsync(AdminLoginDTO dto)
    {
        try
        {
            var admin = await _adminRepository
                .GetAll()
                .FirstOrDefaultAsync(a => a.Email == dto.Email && a.Password == dto.Password);


            if (admin == null) return null;

            return _tokenGenerator.GenerateToken(admin.AdminId.ToString(), admin.Email, admin.Role);
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteAdminAsync(int id)
    {
        try
        {
            var rowsAffected = await _adminRepository.DeleteAsync(id);
            return rowsAffected > 0;
        }
        catch
        {
            return false;
        }
    }

    public Task<Admin> GetAdminByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Admin>> GetAllAdminsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse<ElectionDTO>> CreateElectionAsync(ElectionDTO dto)
    {
        try
        {
            Election election = new Election
            {
                Title = dto.Title,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = true
            };

            int result = await _electionRepository.InsertAsync(election);
            if (result > 0)
            {
                return new ApiResponse<ElectionDTO>(true, "Election Created successfully", dto);
            }
            else
            {
                return new ApiResponse<ElectionDTO>(false, "Failed to create Election");
            }
        }
        catch (Exception ex)
        {
            return new ApiResponse<ElectionDTO>(false, $"Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ElectionDTO>> UpdateElectionAsync(int id, ElectionDTO dto)
    {
        try
        {
            var election = await _electionRepository.GetDetailsAsync(id);
            if (election == null || election.IsActive == false)
            {
                return new ApiResponse<ElectionDTO>(false, "No Election found with this id");
            }
            else
            {
                election.Title = dto.Title;
                election.StartDate = dto.StartDate;
                election.EndDate = dto.EndDate;
            }
            int result = await _electionRepository.UpdateAsync(election);
            if (result > 0)
            {
                return new ApiResponse<ElectionDTO>(true, "election update successfull", dto);
            }
            else
            {
                return new ApiResponse<ElectionDTO>(false, "election update failed");
            }
        }
        catch (Exception ex)
        {
            return new ApiResponse<ElectionDTO>(false, $"Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ElectionDTO>> DeleteElectionAsync(int id)
    {
        try
        {
            var election = await _electionRepository.GetDetailsAsync(id);
            if (election == null || election.IsActive == false)
            {
                return new ApiResponse<ElectionDTO>(false, "No Election found with this id");
            }
            else
            {
                election.IsActive = false;
            }
            int result = await _electionRepository.UpdateAsync(election);
            if (result > 0)
            {
                return new ApiResponse<ElectionDTO>(true, "election delete successfull");
            }
            else
            {
                return new ApiResponse<ElectionDTO>(false, "election delete failed");
            }
        }
        catch (Exception ex)
        {
            return new ApiResponse<ElectionDTO>(false, $"Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<Candidate>> AddCandidateAsync(int id, AddCandidateDTO dto)
    {
        try
        {
            var election = await _electionRepository.GetDetailsAsync(id);
            if (election == null || election.IsActive == false)
                return new ApiResponse<Candidate>(false, "this election is not found , candidate added failed!!");

            var candidate = new Candidate
            {
                Name = dto.Name,
                Party = dto.Party,
                ElectionId = id,
            };

            var existingCandidate = await _candidateRepository
                .GetAll()
                .FirstOrDefaultAsync(c =>
                    c.ElectionId == id &&
                    c.Name.ToLower() == dto.Name.ToLower() &&
                    c.Party.ToLower() == dto.Party.ToLower());

            if (existingCandidate != null)
            {
                return new ApiResponse<Candidate>(false, "Candidate with the same name and party already exists in this election.");
            }

            var result = await _candidateRepository.InsertAsync(candidate);
            if (result == 0)
                return new ApiResponse<Candidate>(false, "Candidate Added Unsuccessfull");

            return new ApiResponse<Candidate>(true, "Candidate added successfully", candidate);
        }
        catch (Exception ex)
        {
            return new ApiResponse<Candidate>(false, $"Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<Candidate>> DeleteCandidateAsync(int electionId, int candidate_id)
    {
        try
        {
            var candidate = await _candidateRepository
                .GetAll()
                .FirstOrDefaultAsync(c => c.CandidateId == candidate_id && c.ElectionId == electionId);

            if (candidate == null)
                return new ApiResponse<Candidate>(false, "no candidate found in this election");

            var result = await _candidateRepository.RemoveAsync(candidate);

            if (result == 0)
                return new ApiResponse<Candidate>(false, "Unsuccessfull");

            return new ApiResponse<Candidate>(true, "successlly delete", candidate);
        }
        catch (Exception ex)
        {
            return new ApiResponse<Candidate>(false, $"Error: {ex.Message}");
        }
    }
}
