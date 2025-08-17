using E_voting.DTO;
using E_voting.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_voting.Controllers
{
    [ApiController]
    [Route("api/voter")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class VoterController : Controller
    {
        private readonly IVoterService _voteService;

        public VoterController(IVoterService voteService)
        {
            _voteService = voteService;
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> registerVoter([FromBody]VoterRegisterDTO registerDTO)
        {
            var response = await _voteService.RegisterVoterAsync(registerDTO);
            if(!response.Success)
                return BadRequest(response);

            return Ok(response);

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> loginVoter([FromBody]VoterLoginDTO loginDTO)
        {
            var token = await _voteService.LoginVoterAsync(loginDTO);
            if (token == null)
                return Unauthorized("Invalid email or password.");

            return Ok(new { Token = token });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteVoter(int id)
        {
            var response = await _voteService.DeleteVoterAsync(id);
            if(!response.Success)
                return BadRequest(response);
            else return Ok(response);
        }

        [HttpPost("{voterId}/vote")]
        public async Task<IActionResult> CastVote(int voterId, [FromBody] CastVoteDTO voteDto)
        {
            var response = await _voteService.CastVoteAsync(voterId, voteDto);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }


    }
}
