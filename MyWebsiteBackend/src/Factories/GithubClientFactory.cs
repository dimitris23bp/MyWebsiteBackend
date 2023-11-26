using Octokit;

namespace MyWebsiteBackend.Factories;

public class GithubClientFactory : IGithubClientFactory
{
    private readonly string _username;

    public GithubClientFactory(IConfiguration configuration)
    {
        _username = configuration.GetValue<string>("Github:Username")!;
        if (_username == null)
        {
            throw new ArgumentException("Github.Username is not set in appsettings.json");
        }
    }

    public IGitHubClient CreateClient()
    {
        return new GitHubClient(new ProductHeaderValue(_username))
        {
            Credentials = new Credentials(Environment.GetEnvironmentVariable("TOKEN_GITHUB"))
        };
    }
}
