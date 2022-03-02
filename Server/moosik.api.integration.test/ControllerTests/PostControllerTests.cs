using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using moosik.api.integration.test.TestUtilities;
using moosik.api.ViewModels;
using moosik.api.ViewModels.Errors;
using moosik.api.ViewModels.Post;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace moosik.api.integration.test.ControllerTests;

[Collection("Docker Container")]
public class PostControllerTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Fixture _fixture;
    private readonly HttpClient _client;

    public PostControllerTests(ContainerManager.ContainerManager manager, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _fixture = new Fixture();
        _client = manager.Factory.CreateClient();
    }

    [Fact]
    public async Task GetAllPost_WhenNoThreadIdPassed_ReturnsSuccessStatusCode()
    {
        // Arrange
        
        // Act
        var response = await _client.GetAsync($"api/Post");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<PostViewModel[]>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));
    }

    [Fact]
    public async Task GetAllPost_WhenValidThreadIdPassed_ReturnsSuccessStatusCode()
    {
        // Arrange
        const int threadId = 1;

        // Act
        var response = await _client.GetAsync($"api/Post?threadId={threadId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<PostViewModel[]>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));
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
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<PostViewModel>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));
        
        
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
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<PostViewModel[]>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));
        
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
    public async Task UpdatePost_WhenValidPostPassed_ReturnsNoContentStatusCode()
    {
        // Arrange
        const int postId = 1;

        var content = new UpdatePostViewModel()
        {
            Description = _fixture.Create<string>()
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"api/post/{postId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task UpdatePost_WhenInvalidPostPassed_ReturnsBadRequestStatusCode()
    {
        // Arrange
        const int postId = 1;

        var content = new UpdatePostViewModel()
        {
            
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"api/post/{postId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<BadRequestViewModel>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));
        
    }
    
    [Fact]
    public async Task UpdatePost_WhenInvalidPostId_ReturnsNotFoundStatusCode()
    {
        // Arrange
        const int postId = 99;

        var content = new UpdatePostViewModel()
        {
            Description = _fixture.Create<string>()
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"api/post/{postId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreatePost_WhenValidPostPassed_ReturnsNoContentStatusCode()
    {
        // Arrange
        const int threadId = 1;
        
        var createPostViewModel = new CreatePostViewModel()
        {
            Description = _fixture.Create<string>(),
            UserId = 1,
            Resource = new CreatePostResourceViewModel()
            {
                Title = _fixture.Create<string>(),
                TypeId = 1,
                Value = _fixture.Create<string>()
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/post/{threadId}", createPostViewModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [InlineData(0, "")]
    [InlineData(0, "testDescription")]
    [InlineData(1, "")]
    public async Task CreatePost_WhenInvalidPostPassed_ReturnsBadRequestStatusCode(int userId, string description)
    {
        // Arrange
        const int threadId = 1;
        
        var createPostViewModel = new CreatePostViewModel()
        {
            UserId = userId,
            Description = description,
            Resource = new CreatePostResourceViewModel()
            {
                Title = _fixture.Create<string>(),
                TypeId = 1,
                Value = _fixture.Create<string>()
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/post/{threadId}", createPostViewModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var responseData = await response.Content.ReadAsStringAsync();
        responseData.VerifyDeSerialization<BadRequestViewModel>();
        
        _testOutputHelper.WriteLine(JToken.Parse(responseData).ToString(Formatting.Indented));
    }

    [Fact]
    public async Task CreatePost_WhenInvalidThreadId_ReturnsInternalServerErrorStatusCode()
    {
        // Arrange
        const int threadId = 99;
        
        var createPostViewModel = new CreatePostViewModel()
        {
            Description = _fixture.Create<string>(),
            UserId = 1,
            Resource = new CreatePostResourceViewModel()
            {
                Title = _fixture.Create<string>(),
                TypeId = 1,
                Value = _fixture.Create<string>()
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/post/{threadId}", createPostViewModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task DeletePost_WhenValidIdPassed_ReturnsNoContentStatusCode()
    {
        // Arrange
        const int postId = 1;

        // Act
        var response = await _client.DeleteAsync($"api/Post/{postId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task DeletePost_WhenInvalidIdPassed_ReturnsNotFoundErrorStatusCode()
    {
        // Arrange
        const int postId = 99;

        // Act
        var response = await _client.DeleteAsync($"api/Post/{postId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task UpdatePostResource_WhenValidPostResourcePassed_ReturnsNoContentStatusCode()
    {
        // Arrange
        const int postResourceId = 1;

        var content = new UpdatePostResourceViewModel()
        {
            Title = _fixture.Create<string>(),
            Value = _fixture.Create<string>()
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"api/post/resources/{postResourceId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [InlineData("", "")]
    [InlineData("TestTitle","")]
    [InlineData("", "TestValue")]
    public async Task UpdatePostResource_WhenInvalidPostResourcePassed_ReturnsBadRequestStatusCode(string title, string value)
    {
        // Arrange
        const int postResourceId = 1;

        var content = new UpdatePostResourceViewModel()
        {
            Title = title,
            Value = value
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"api/post/resources/{postResourceId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var responseData = await response.Content.ReadAsStringAsync();
        responseData.VerifyDeSerialization<BadRequestViewModel>();
        
        _testOutputHelper.WriteLine(JToken.Parse(responseData).ToString(Formatting.Indented));
    }
    
    [Fact]
    public async Task UpdatePostResource_WhenInvalidPostResourceId_ReturnsNotFoundStatusCode()
    {
        // Arrange
        const int postResourceId = 99;

        var content = new UpdatePostResourceViewModel()
        {
            Title = _fixture.Create<string>(),
            Value = _fixture.Create<string>()
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"api/post/resources/{postResourceId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllResourceTypes_WhenNoParamsPassed_ReturnsSuccessStatusCode()
    {
        // Arrange
        
        // Act
        var response = await _client.GetAsync("api/Post/resource-types");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var value = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(value.VerifyDeSerialization<ResourceTypeViewModel[]>());

    }
}