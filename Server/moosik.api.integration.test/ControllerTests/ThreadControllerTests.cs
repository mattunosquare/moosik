using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using moosik.api.integration.test.TestUtilities;
using moosik.api.ViewModels;
using moosik.api.ViewModels.Errors;
using moosik.api.ViewModels.Thread;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace moosik.api.integration.test.ControllerTests;

[Collection("Docker Container")]
public class ThreadControllerTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Fixture _fixture;
    private readonly HttpClient _client;

    public ThreadControllerTests(ContainerManager.ContainerManager manager, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _fixture = new Fixture();
        _client = manager.Factory.CreateClient();
    }
    
    [Fact]
    public async Task GetAllThreads_WhenNoUserIdPassed_ReturnsSuccessStatusCode()
    {
        // Arrange
        
        // Act
        var response = await _client.GetAsync($"api/thread");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<ThreadViewModel[]>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));
    }

    [Fact]
    public async Task GetAllThreads_WhenValidUserIdPassed_ReturnsSuccessStatusCode()
    {
        // Arrange
        const int userId = 1;

        // Act
        var response = await _client.GetAsync($"api/Thread?userId={userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<ThreadViewModel[]>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));
    }

    [Fact]
    public async Task GetAllThreads_WhenNoThreadFound_ReturnsNoContentStatusCode()
    {
        // Arrange
        const int userId = 99;

        // Act
        var response = await _client.GetAsync($"api/Thread?userId={userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task GetThreadById_WhenValidIdPassed_ReturnsSuccessStatusCode()
    {
        // Arrange
        const int threadId = 1;
        
        // Act
        var response = await _client.GetAsync($"api/thread/{threadId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<ThreadViewModel>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));
        
    }

    [Fact]
    public async Task GetThreadById_WhenNoThreadFound_ReturnsNotFoundStatusCode()
    {
        // Arrange
        const int threadId = 99;
        
        // Act
        var response = await _client.GetAsync($"api/thread/{threadId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetThreadById_WhenInvalidIdPassed_ReturnsNotFoundStatusCode()
    {
        // Arrange
        const int threadId = -99;

        // Act
        var response = await _client.GetAsync($"api/thread/{threadId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetThreadsAfterDate_WhenThreadsFound_ReturnsSuccessStatusCode()
    {
        // Arrange
        const string dateStr = "2011-11-24 14:18:29";

        // Act
        var response = await _client.GetAsync($"api/thread/get-after-date?date={dateStr}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<ThreadViewModel[]>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));
        
    }
    
    [Fact]
    public async Task GetThreadsAfterDate_WhenNoThreadsFound_ReturnsNoContentStatusCode()
    {
        // Arrange
        const string dateStr = "2111-11-24 14:18:29";

        // Act
        var response = await _client.GetAsync($"api/thread/get-after-date?date={dateStr}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

    }
    
    [Fact]
    public async Task GetThreadsAfterDate_WhenInvalidDatePassed_ReturnsBadRequestStatusCode()
    {
        // Arrange
        const string dateStr = "Invalid Date";

        // Act
        var response = await _client.GetAsync($"api/thread/get-after-date?date={dateStr}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task UpdateThread_WhenValidThreadPassed_ReturnsNoContentStatusCode()
    {
        // Arrange
        const int threadId = 1;

        var content = new UpdateThreadViewModel()
        {
            Title = _fixture.Create<string>()
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"api/thread/{threadId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task UpdateThread_WhenInvalidThreadPassed_ReturnsBadRequestStatusCode()
    {
        // Arrange
        const int threadId = 1;
        
        var content = new UpdateThreadViewModel()
        {
            
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"api/thread/{threadId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<BadRequestViewModel>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));
        
    }
    
    [Fact]
    public async Task UpdateThread_WhenInvalidThreadId_ReturnsNotFoundStatusCode()
    {
        // Arrange
        const int threadId = 99;

        var content = new UpdateThreadViewModel()
        {
            Title = _fixture.Create<string>()
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"api/thread/{threadId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task CreateThread_WhenValidThreadPassed_ReturnsNoContentStatusCode()
    {
        // Arrange

        var createThreadViewModel = new CreateThreadViewModel()
        {
            Title = _fixture.Create<string>(),
            UserId = 1,
            ThreadTypeId = 1,
            Post = new CreatePostViewModel()
            {
                Description = _fixture.Create<string>(),
                UserId = 1
            }
        };
        
        // Act
        var response = await _client.PostAsJsonAsync($"/api/thread/", createThreadViewModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task CreateThread_WhenInvalidThreadPassed_ReturnsBadRequestStatusCode()
    {
        // Arrange

        var createPostViewModel = new CreatePostViewModel()
        {
           
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/thread", createPostViewModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var responseData = await response.Content.ReadAsStringAsync();
        responseData.VerifyDeSerialization<BadRequestViewModel>();
        
        _testOutputHelper.WriteLine(JToken.Parse(responseData).ToString(Formatting.Indented));
    }

    [Fact]
    public async Task DeleteThread_WhenValidIdPassed_ReturnsNoContentStatusCode()
    {
        // Arrange
        const int threadId = 1;

        // Act
        var response = await _client.DeleteAsync($"api/thread/{threadId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task DeleteThread_WhenInvalidIdPassed_ReturnsNotFoundErrorStatusCode()
    {
        // Arrange
        const int threadId = 99;

        // Act
        var response = await _client.DeleteAsync($"api/thread/{threadId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetAllThreadTypes_WhenNoParamsPassed_ReturnsSuccessStatusCode()
    {
        // Arrange
        
        // Act
        var response = await _client.GetAsync("api/thread/types");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<ThreadTypeViewModel[]>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));

    }
}