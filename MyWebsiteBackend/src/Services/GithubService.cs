using Microsoft.Extensions.Caching.Memory;
using MyWebsiteBackend.Services.Interfaces;
using Octokit;

namespace MyWebsiteBackend.Services;

public class GithubService : IGithubService
{
    private readonly ILogger<GithubService> _logger;
    private readonly IMemoryCache _cache;

    private readonly GitHubClient Client;
    private readonly string Username = "dimitris23bp";

    public GithubService(ILogger<GithubService> logger, IMemoryCache cache)
    {
        _logger = logger;
        _cache = cache;
        Client = new GitHubClient(new ProductHeaderValue(Username))
        {
            Credentials = new Credentials(Environment.GetEnvironmentVariable("TOKEN_GITHUB"))
        };
    }

    public async Task<SearchRepositoryResult> GetGithubRepositories()
    {
        var request = new SearchRepositoriesRequest() { User = Username };
        return await GetGithubRepositories(request);
    }

    /**
    * Get Github Repositories
    * There is a cache mechanism in place, because I use that method plenty of times with the same exact request
    *
    * @param request
    * @return SearchRepositoryResult
    */
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
        var response = await Client.Search.SearchRepo(request);
        _logger.LogDebug($"Response from GetGithubRepositories: {response}");

        // TODO: Make seconds a global variable
        _cache.Set(cacheValue, response, TimeSpan.FromSeconds(30));
        return response;
    }

    public async Task<Dictionary<string, long>> GetAllLanguagesQuotas()
    {
        var response = await GetGithubRepositories();
        var totalLanguages = new Dictionary<string, long>();
        foreach (var repo in response.Items)
        {
            var languages = await Client.Repository.GetAllLanguages(repo.Id);
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

    public async Task<Dictionary<string, int>> GetAllLanguages()
    {
        var response = await GetGithubRepositories();

        var totalLanguages = new Dictionary<string, int>();
        foreach (var repo in response.Items)
        {
            var languages = await Client.Repository.GetAllLanguages(repo.Id);
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

    public async Task<Dictionary<string, int>> GetMainLanguages()
    {
        var response = await GetGithubRepositories();

        var mainLanguages = new Dictionary<string, int>();
        response
            .Items
            .ToList()
            .ForEach(repo =>
            {
                AddLanguage(mainLanguages, repo.Language);
            });

        return mainLanguages;
    }

    public async Task<Repository> GetRepository(string repoName)
    {
        var response = await Client.Repository.Get(Username, repoName);
        _logger.LogDebug($"Response from GetGithubRepositories for {repoName}: {response}");
        return response;
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
