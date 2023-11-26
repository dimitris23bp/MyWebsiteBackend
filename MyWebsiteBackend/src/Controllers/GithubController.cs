using Microsoft.AspNetCore.Mvc;
using MyWebsiteBackend.Services.Interfaces;

namespace MyWebsiteBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class GithubController : ControllerBase
{
    private readonly ILogger<GithubController> _logger;
    private readonly IGithubService _githubService;

    public GithubController(ILogger<GithubController> logger, IGithubService githubService)
    {
        _logger = logger;
        _githubService = githubService;
    }

    [HttpGet("repositories")]
    public async Task<IActionResult> GetGithubRepositories()
    {
        var response = await _githubService.GetGithubRepositories();

        return Ok(response);
    }

    [HttpGet("languages/quotas")]
    public async Task<IActionResult> GetAllLanguagesQuota()
    {
        var response = await _githubService.GetAllLanguagesQuotas();
        return Ok(response);
    }

    [HttpGet("languages/all")]
    public async Task<IActionResult> GetAllLanguages()
    {
        var response = await _githubService.GetAllLanguages();
        return Ok(response);
    }

    [HttpGet("languages/main")]
    public async Task<IActionResult> GetMainLanguages()
    {
        var response = await _githubService.GetMainLanguages();
        return Ok(response);
    }

    [HttpGet("repository/{repoName}")]
    public async Task<IActionResult> GetGithubRepository(string repoName)
    {
        var response = await _githubService.GetRepository(repoName);
        return Ok(response);
    }

    [HttpGet("commits")]
    public async Task<IActionResult> GetCommitsAmount()
    {
        var response = await _githubService.GetCommits();
        return Ok(response);
    }
}
