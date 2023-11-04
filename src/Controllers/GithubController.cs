using Microsoft.AspNetCore.Mvc;
using Octokit;

namespace MyWebsiteBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GithubController : ControllerBase
    {
        private readonly GitHubClient Client;
        private readonly string Username = "dimitris23bp";

        public GithubController()
        {
            Client = new GitHubClient(new ProductHeaderValue(Username))
            {
                Credentials = new Credentials(Environment.GetEnvironmentVariable("TOKEN_GITHUB"))
            };
        }

        [HttpGet("repositories")]
        public async Task<IActionResult> GetGitHubRepo()
        {
            var request = new SearchRepositoriesRequest()
            {
                User = Username
            };
            var response = await Client.Search.SearchRepo(request);

            return Ok(response);
        }

        [HttpGet("repository/{repoName}")]
        public async Task<IActionResult> GetGitHubRepo(string repoName)
        {
            var response = await Client.Repository.Get(Username, repoName);

            return Ok(response);
        }
    }
}