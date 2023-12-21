using Microsoft.Extensions.Caching.Memory;
using MyWebsiteBackend.Factories;
using MyWebsiteBackend.Services.Interfaces;
using Octokit;

namespace MyWebsiteBackend.Services;

public class GithubService : IGithubService
{
    private readonly ILogger<GithubService> _logger;
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _configuration;

    private readonly IGitHubClient _client;
    private readonly string Username;

    public GithubService(
        ILogger<GithubService> logger,
        IMemoryCache cache,
        IConfiguration configuration,
        IGithubClientFactory githubClientFactory
    )
    {
        _logger = logger;
        _cache = cache;
        _configuration = configuration;
        _client = githubClientFactory.CreateClient();
        Username = _configuration.GetValue<string>("Github:Username")!;
        if (Username == null)
        {
            throw new ArgumentException("Github.Username is not set in appsettings.json");
        }
    }

    public async Task<SearchRepositoryResult> GetGithubRepositories()
    {
        var request = new SearchRepositoriesRequest() { User = Username };
        return await GetGithubRepositories(request);
    }

    /// <summary>
    /// Get Github Repositories
    /// There is a cache mechanism in place, because I use that method plenty of times with the same exact request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<SearchRepositoryResult> GetGithubRepositories(
        SearchRepositoriesRequest request
    )
    {
        var cacheValue = $"github-repositories{request.User}";
        if (_cache.TryGetValue(cacheValue, out SearchRepositoryResult? value))
        {
            return value!;
        }
        _logger.LogDebug($"Request from GetGithubRepositories: {request}");
        var response = await _client.Search.SearchRepo(request);
        _logger.LogDebug($"Response from GetGithubRepositories: {response}");

        var seconds = _configuration.GetValue<int>("Github:CacheDurationInSeconds");
        _cache.Set(cacheValue, response, TimeSpan.FromSeconds(seconds));
        return response;
    }

    /// <summary>
    /// Get all languages from all repositories
    /// </summary>
    /// <returns>
    /// A dictionary with all languages that are used as key, and their overall number of bytes  as value
    /// </returns>
    public async Task<Dictionary<string, long>> GetAllLanguagesQuotas()
    {
        var response = await GetGithubRepositories();
        var totalLanguages = new Dictionary<string, long>();
        foreach (var repo in response.Items)
        {
            var languages = await _client.Repository.GetAllLanguages(repo.Id);
            languages
                .ToList()
                .ForEach(language =>
                {
                    AddAllLanguagesQuotas(totalLanguages, language);
                    _logger.LogDebug($"{repo.Name}: {language.Name}");
                });
        }
        return totalLanguages;
    }

    /// <summary>
    /// Get all languages from all repositories
    /// </summary>
    /// <returns>
    /// A dictionary with all languages as key and the amount of times a language was used as value
    /// </returns>
    public async Task<Dictionary<string, int>> GetAllLanguages()
    {
        var response = await GetGithubRepositories();

        var totalLanguages = new Dictionary<string, int>();
        foreach (var repo in response.Items)
        {
            var languages = await _client.Repository.GetAllLanguages(repo.Id);
            languages
                .ToList()
                .ForEach(language =>
                {
                    AddLanguage(totalLanguages, language.Name);
                    _logger.LogDebug($"{repo.Name}: {language.Name}");
                });
        }

        return totalLanguages;
    }

    /// <summary>
    /// Get the main languages from all repositories
    /// </summary>
    /// <returns>
    /// A dictionary with all languages as key and the amount of times a language was used as primary language on a repo as value
    /// </returns>
    public async Task<Dictionary<string, int>> GetMainLanguages()
    {
        var response = await GetGithubRepositories();

        var mainLanguages = new Dictionary<string, int>();
        response
            .Items.ToList()
            .ForEach(repo =>
            {
                AddLanguage(mainLanguages, repo.Language);
            });

        return mainLanguages;
    }

    /// <summary>
    /// Get a specific repository
    /// </summary>
    /// <param name="repoName">Name of the repository</param>
    /// <returns>Repository</returns>
    public async Task<Repository> GetRepository(string repoName)
    {
        var response = await _client.Repository.Get(Username, repoName);
        _logger.LogDebug($"Response from GetGithubRepositories for {repoName}: {response}");
        return response;
    }

    /// <summary>
    /// Get total number of commits from all repositories of user
    /// </summary>
    /// <returns>The amount of commits</returns>
    public async Task<int> GetCommits()
    {
        var repositories = await GetGithubRepositories();

        var tasks = repositories.Items.Select(async repo =>
        {
            var response = await _client.Repository.Commit.GetAll(Username, repo.Name);
            _logger.LogDebug($"Response from GetCommits for {repo.Name}: {response.Count}");
            return response.Count;
        });
        var results = Task.WhenAll(tasks);
        return results.Result.Sum();
    }

    private void AddLanguage(Dictionary<string, int> mainLanguages, string name)
    {
        mainLanguages[name] = mainLanguages.ContainsKey(name) ? mainLanguages[name] + 1 : 1;
    }

    private void AddAllLanguagesQuotas(
        Dictionary<string, long> languages,
        RepositoryLanguage language
    )
    {
        if (languages.ContainsKey(language.Name))
        {
            languages[language.Name] += language.NumberOfBytes;
        }
        else
        {
            languages.Add(language.Name, language.NumberOfBytes);
        }
    }
}
