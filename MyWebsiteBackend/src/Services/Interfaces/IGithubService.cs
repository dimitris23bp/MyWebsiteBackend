using Octokit;

namespace MyWebsiteBackend.Services.Interfaces;

public interface IGithubService
{
    public Task<SearchRepositoryResult> GetGithubRepositories();
    public Task<SearchRepositoryResult> GetGithubRepositories(SearchRepositoriesRequest request);
    public Task<Dictionary<string, long>> GetAllLanguagesQuotas();
    public Task<Dictionary<string, int>> GetAllLanguages();
    public Task<Dictionary<string, int>> GetMainLanguages();
    public Task<Repository> GetRepository(string repoName);
}
