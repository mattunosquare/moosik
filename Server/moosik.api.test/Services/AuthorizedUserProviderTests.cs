using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using moosik.api.Authorization.Services;
using moosik.services.Dtos;
using moosik.services.Interfaces;
using NSubstitute;
using Xunit;

namespace moosik.api.test.Services;

public class AuthorizedUserProviderTests
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Fixture _fixture;
    
    private UserDto? _user;
    
    public AuthorizedUserProviderTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        _userService = Substitute.For<IUserService>();
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    }

    [Fact]
    public void GetLoggedInUser_WhenUserLoggedIn_UserReturned()
    {
        // Arrange
        const string userId = "1";

        var identity = new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.NameIdentifier, userId)
        });
        
        _httpContextAccessor.HttpContext?.User.Returns(new ClaimsPrincipal(identity));
        
        _user = _fixture.Build<UserDto>().Create();

        _userService.GetAllUsers(int.Parse(userId)).Returns(new []
        {
            _user
        });
        
        var service = GetService();

        // Act
        var result = service.GetLoggedInUser();

        // Assert
        result.Should().BeEquivalentTo(_user);
    }

    [Fact]
    public void GetLoggedInUser_WhenNoUserLoggedIn_ReturnsNull()
    {
        // Arrange

        var service = GetService();

        // Act
        var result = service.GetLoggedInUser();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetLoggedInUser_WhenUserAlreadySet_UserReturned()
    {
        // Arrange
        var service = GetService();
        service._user = _fixture.Build<UserDto>().Create();
        
        // Assert
        var result = service.GetLoggedInUser();

        // Act
        result.Should().BeEquivalentTo(service._user);
    }
    
    
    private AuthorizedUserProvider GetService()
    {
        return new AuthorizedUserProvider(_userService, _httpContextAccessor);
    }
}