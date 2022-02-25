using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using moosik.dal.Models;
using Xunit;
using Xunit.Abstractions;

namespace moosik.api.integration.test.ControllerTests;

[Collection("Docker Container")]
public class PostControllerTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ContainerManager.ContainerManager _containerManager;
    private readonly Fixture _fixture;
    private readonly HttpClient _client;

    public PostControllerTest(ContainerManager.ContainerManager manager, ITestOutputHelper testOutputHelper)
    {
        _containerManager = manager;
        _testOutputHelper = testOutputHelper;
        _fixture = new Fixture();

        _client = _containerManager.Factory.CreateClient();
    }

    [Fact]
    public async Task GetAllPost_WhenNoThreadIdPassed_ReturnsSuccessStatusCode()
    {
        // Arrange
        
        // Act
        var response = await _client.GetAsync($"api/Post");

        // Assert
        var value = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(value);

        response.Should().BeSuccessful();
    }

    [Fact]
    public async Task GetAllPost_WhenValidThreadIdPassed_ReturnsSuccessStatusCode()
    {
        // Arrange
        const int threadId = 1;

        // Act
        var response = await _client.GetAsync($"api/Post?threadId={threadId}");

        // Assert
        var value = await response.Content.ReadAsStringAsync();
        
        _testOutputHelper.WriteLine(value);
        
        response.Should().BeSuccessful();
    }

    [Fact]
    public async Task GetAllPost_WhenNoPostFound_ReturnsNoContentStatusCode()
    {
        // Arrange
        const int threadId = 99;

        // Act
        var response = await _client.GetAsync($"api/Post?threadId={threadId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetPostById_WhenValidIdPassed_ReturnsSuccessStatusCode()
    {
        // Arrange
        const int postId = 1;
        
        // Act
        var response = await _client.GetAsync($"api/Post/{postId}");

        // Assert
        var value = await response.Content.ReadAsStringAsync();
        
        _testOutputHelper.WriteLine(value);
        
        response.Should().BeSuccessful();
    }

    [Fact]
    public async Task GetPostById_WhenNoPostFound_ReturnsNotFoundStatusCode()
    {
        // Arrange
        const int postId = 99;
        
        // Act
        var response = await _client.GetAsync($"api/Post/{postId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPostById_WhenInvalidIdPassed_ReturnsNotFoundStatusCode()
    {
        // Arrange
        const int postId = -99;

        // Act
        var response = await _client.GetAsync($"api/Post/{postId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPostsAfterDate_WhenPostsFound_ReturnsSuccessStatusCode()
    {
        // Arrange
        const string dateStr = "2011-11-24 14:18:29";

        // Act
        var response = await _client.GetAsync($"api/Post/get-after-date?date={dateStr}");

        // Assert
        var value = await response.Content.ReadAsStringAsync();
        
        _testOutputHelper.WriteLine(value);
        
        response.Should().BeSuccessful();
    }
    
    [Fact]
    public async Task GetPostsAfterDate_WhenNoPostsFound_ReturnsNoContentStatusCode()
    {
        // Arrange
        const string dateStr = "2111-11-24 14:18:29";

        // Act
        var response = await _client.GetAsync($"api/Post/get-after-date?date={dateStr}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

    }
    
    [Fact]
    public async Task GetPostsAfterDate_WhenInvalidDatePassed_ReturnsBadRequestStatusCode()
    {
        // Arrange
        const string dateStr = "Invalid Date";

        // Act
        var response = await _client.GetAsync($"api/Post/get-after-date?date={dateStr}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdatePost_WhenValidPostPassed_ReturnsNoContent()
    {
        
    }
}