using E_voting.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace E_voting.Controllers;

[ApiController]
[Route("api")]
public class HomeController : Controller
{
    private readonly IElectionService _electionService;

    public HomeController(IElectionService electionService)
    {
        _electionService = electionService;
    }

    [HttpGet("election")]
    public async Task<IActionResult> getAllElections()
    {
        var response =await _electionService.GetAllElectionsAsync();
        if(!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpGet("election/{id}")]
    public async Task<IActionResult> getElectionByID(int id)
    {
        var response = await _electionService.GetElectionByIdAsync(id);

        if(!response.Success)
            return BadRequest(response);
        return Ok(response);
    }

    [HttpGet("election/{id}/candidateList")]
    public async Task<IActionResult> GetCandidatesByElectionId(int id)
    {
        var response =await _electionService.GetCandidateListByElectionIdAsync(id);

        if(!response.Success)
            return BadRequest(response);
        return Ok(response);
    }


}
