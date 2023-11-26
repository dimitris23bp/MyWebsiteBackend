using Octokit;

namespace MyWebsiteBackend.Factories;

public interface IGithubClientFactory
{
    IGitHubClient CreateClient();
}
