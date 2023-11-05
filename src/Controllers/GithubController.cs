using Microsoft.AspNetCore.Mvc;
using Octokit;

namespace MyWebsiteBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class GithubController : ControllerBase
{
    private readonly ILogger<GithubController> _logger;

    private readonly GitHubClient Client;
    private readonly string Username = "dimitris23bp";

    public GithubController(ILogger<GithubController> logger)
    {
        this._logger = logger;
        Client = new GitHubClient(new ProductHeaderValue(Username))
        {
            Credentials = new Credentials(Environment.GetEnvironmentVariable("TOKEN_GITHUB"))
        };
    }

    [HttpGet("repositories")]
    public async Task<IActionResult> GetGithubRepositories()
    {
        var request = new SearchRepositoriesRequest()
        {
            User = Username
        };
        _logger.LogDebug($"Request from GetGithubRepositories: {request}");
        var response = await Client.Search.SearchRepo(request);
        _logger.LogDebug($"Response from GetGithubRepositories: {response}");

        return Ok(response);
    }

    [HttpGet("repository/{repoName}")]
    public async Task<IActionResult> GetGitHubRepo(string repoName)
    {
        var response = await Client.Repository.Get(Username, repoName);
        _logger.LogDebug($"Response from GetGithubRepositories for {repoName}: {response}");

        return Ok(response);
    }
}