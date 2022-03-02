using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using moosik.api.integration.test.TestUtilities;
using moosik.api.ViewModels.Errors;
using moosik.api.ViewModels.User;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace moosik.api.integration.test.ControllerTests;

[Collection("Docker Container")]
public class UserControllerTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Fixture _fixture;
    private readonly HttpClient _client;
    
    public UserControllerTests(ContainerManager.ContainerManager manager, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _fixture = new Fixture();
        _client = manager.Factory.CreateClient();
    }
    
    [Fact]
    public async Task GetAllUsers_WhenNoUserIdPassed_ReturnsSuccessStatusCode()
    {
        // Arrange
        
        // Act
        var response = await _client.GetAsync($"api/user");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<UserViewModel[]>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));
    }

    [Fact]
    public async Task GetAllUsers_WhenValidUserIdPassed_ReturnsSuccessStatusCode()
    {
        // Arrange
        const int userId = 1;

        // Act
        var response = await _client.GetAsync($"api/user?userId={userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<UserViewModel[]>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));
    }

    [Fact]
    public async Task GetAllUsers_WhenNoThreadFound_ReturnsNoContentStatusCode()
    {
        // Arrange
        const int userId = 99;

        // Act
        var response = await _client.GetAsync($"api/user?userId={userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task GetDetailedUserById_WhenValidIdPassed_ReturnsSuccessStatusCode()
    {
        // Arrange
        const int userId = 1;
        
        // Act
        var response = await _client.GetAsync($"api/user/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<UserDetailViewModel>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));
        
    }

    [Fact]
    public async Task GetDetailedUserById_WhenNoUserFound_ReturnsNotFoundStatusCode()
    {
        // Arrange
        const int userId = 99;
        
        // Act
        var response = await _client.GetAsync($"api/user/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetDetailedUserById_WhenInvalidIdPassed_ReturnsNotFoundStatusCode()
    {
        // Arrange
        const int userId = -99;

        // Act
        var response = await _client.GetAsync($"api/user/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetUserByUsernameAndEmail_WhenUserFound_ReturnsSuccessStatusCode()
    {
        // Arrange
        const string username = "mirdaleb";
        const string email = "asifflettq@vimeo.com";

        // Act
        var response = await _client.GetAsync($"api/user/get-user-by-username-and-email?username={username}&email={email}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<UserViewModel>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));
        
    }
    
    [Fact]
    public async Task GetUserByUsernameAndEmail_WhenNoUserFound_ReturnsNoContentStatusCode()
    {
        // Arrange
        const string username = "noUser";
        const string email = "noEmail@email.com";

        // Act
        var response = await _client.GetAsync($"api/user/get-user-by-username-and-email?username={username}&email={email}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

    }
    
    [Fact]
    public async Task UpdateUser_WhenValidUserPassed_ReturnsNoContentStatusCode()
    {
        // Arrange
        const int userId = 1;

        var content = new UpdateUserViewModel()
        {
            Email = "newEmail@email.com",
            Username = "NewUsername"
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"api/user/{userId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task UpdateUser_WhenInvalidUserPassed_ReturnsBadRequestStatusCode()
    {
        // Arrange
        const int userId = 1;

        var content = new UpdateUserViewModel()
        {
           
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"api/user/{userId}", content);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var value = await response.Content.ReadAsStringAsync();
        value.VerifyDeSerialization<BadRequestViewModel>();
        _testOutputHelper.WriteLine(JToken.Parse(value).ToString(Formatting.Indented));
        
    }
    
    [Fact]
    public async Task UpdateUser_WhenInvalidUserId_ReturnsNotFoundStatusCode()
    {
        // Arrange
        const int userId = 99;

        var content = new UpdateUserViewModel()
        {
            Email = "newEmail@email.com",
            Username = "NewUsername"
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"api/user/{userId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task CreateUser_WhenValidUserPassed_ReturnsNoContentStatusCode()
    {
        // Arrange

        var createUserViewModel = new CreateUserViewModel()
        {
            Username = _fixture.Create<string>(),
            Email = "createdEmail@email.com",
            Password = "$2a$12$KEWBhOFSHXyMjWx6yvnfa.g6yx0rbmg5wi1.Sf9Q5FdovQEyKyTna",
            RoleId = 1
        };
        
        // Act
        var response = await _client.PostAsJsonAsync($"/api/user/", createUserViewModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task CreateUser_WhenInvalidUserPassed_ReturnsBadRequestStatusCode()
    {
        // Arrange

        var createUserViewModel = new CreateUserViewModel()
        {
            
        };
        
        // Act
        var response = await _client.PostAsJsonAsync($"/api/user/", createUserViewModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var responseData = await response.Content.ReadAsStringAsync();
        responseData.VerifyDeSerialization<BadRequestViewModel>();
        
        _testOutputHelper.WriteLine(JToken.Parse(responseData).ToString(Formatting.Indented));
    }
    
    [Fact]
    public async Task DeleteUser_WhenValidIdPassed_ReturnsNoContentStatusCode()
    {
        // Arrange
        const int userId = 1;

        // Act
        var response = await _client.DeleteAsync($"api/user/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task DeleteUser_WhenInvalidIdPassed_ReturnsNotFoundErrorStatusCode()
    {
        // Arrange
        const int userId = 99;

        // Act
        var response = await _client.DeleteAsync($"api/user/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

