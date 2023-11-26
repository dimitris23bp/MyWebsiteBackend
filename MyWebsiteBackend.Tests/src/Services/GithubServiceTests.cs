using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MyWebsiteBackend.Factories;
using MyWebsiteBackend.Services;
using Octokit;

namespace MyWebsiteBackend.Tests.Services;

public class GithubServiceTests
{
    private readonly Mock<IGitHubClient> _clientMock;
    private readonly Mock<ILogger<GithubService>> _loggerMock;

    private readonly Mock<IMemoryCache> _cacheMock;
    private readonly Mock<IGithubClientFactory> _clientFactoryMock;
    private readonly GithubService _githubService;

    public GithubServiceTests()
    {
        _clientMock = new Mock<IGitHubClient>();
        _clientFactoryMock = new Mock<IGithubClientFactory>();
        _clientFactoryMock.Setup(f => f.CreateClient()).Returns(_clientMock.Object);
        _loggerMock = new Mock<ILogger<GithubService>>();
        _cacheMock = new Mock<IMemoryCache>();
        var myConfiguration = new Dictionary<string, string>
        {
            { "Github:Username", "dimitris23bp" },
            { "Github:CacheDurationInSeconds", "30" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                myConfiguration.Select(kv => new KeyValuePair<string, string?>(kv.Key, kv.Value))
            )
            .Build();
        _githubService = new GithubService(
            _loggerMock.Object,
            _cacheMock.Object,
            configuration,
            _clientFactoryMock.Object
        );
    }

    [Fact]
    public async Task GetGithubRepositories_ReturnsRepositories_WhenCacheIsEmpty()
    {
        // Arrange
        var request = new SearchRepositoriesRequest() { User = "dimitris23bp" };
        var expectedResponse = new SearchRepositoryResult(
            totalCount: 1,
            incompleteResults: false,
            items: new List<Repository>()
        );
        _clientMock.Setup(c => c.Search.SearchRepo(request)).ReturnsAsync(expectedResponse);
        _cacheMock
            .Setup(c => c.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
            .Returns(false);
        _cacheMock
            .Setup(c => c.CreateEntry(It.IsAny<object>()))
            .Returns(new Mock<ICacheEntry>().Object);

        // Act
        var result = await _githubService.GetGithubRepositories(request);

        // Assert
        Assert.Equal(expectedResponse, result);
        _clientMock.Verify(c => c.Search.SearchRepo(request), Times.Once);
        _cacheMock.Verify(c => c.CreateEntry(It.IsAny<object>()), Times.Once);
    }
}
