using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using moosik.api.Authentication.Interfaces;
using moosik.api.Authorization.Interfaces;
using moosik.api.Controllers;
using moosik.api.test.Extensions;
using moosik.api.ViewModels.Authentication;
using moosik.services.Dtos;
using moosik.services.Dtos.Authentication;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace moosik.api.test.Controllers;

public class AuthenticationControllerTests
{
    private readonly IAuthenticationService _authService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IAuthorizedUserProvider _authorizedUserProvider;
    private readonly Fixture _fixture;

    public AuthenticationControllerTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        _authService = Substitute.For<IAuthenticationService>();
        _tokenService = Substitute.For<ITokenService>();
        _mapper = Substitute.For<IMapper>();
        _authorizedUserProvider = Substitute.For<IAuthorizedUserProvider>();
    }

    [Fact]
    public void Authenticate_WhenAuthPasses_MapsAndReturnsAuthenticationResponseViewModel()
    {
        // Arrange
        var authenticationRequestViewModel = _fixture.Build<AuthenticationRequestViewModel>().Create();
        var authenticationRequestDto = _fixture.Build<AuthenticationRequestDto>().Create();

        _mapper.Map<AuthenticationRequestDto>(authenticationRequestViewModel).Returns(authenticationRequestDto);
        
        var userDto = _fixture.Build<UserDto>().Create();
        _authService.Authenticate(authenticationRequestDto).Returns(userDto);

        var authenticationResponseDto = _fixture.Build<AuthenticationResponseDto>().Create();
        _tokenService.AppendTokens(userDto).Returns(authenticationResponseDto);
        
        var authenticationResponseViewModel = _fixture.Build<AuthenticationResponseViewModel>().Create();

        _mapper.Map<AuthenticationResponseViewModel>(authenticationResponseDto)
            .Returns(authenticationResponseViewModel);

        var controller = GetController();

        // Act
        var actionResult = controller.Authenticate(authenticationRequestViewModel);

        // Assert
        var result = actionResult.AssertObjectResult<AuthenticationResponseViewModel, OkObjectResult>();
        result.Should().BeSameAs(authenticationResponseViewModel);

        _mapper.Received(1).Map<AuthenticationRequestDto>(authenticationRequestViewModel);

        _mapper.Received(1).Map<AuthenticationResponseViewModel>(authenticationResponseDto);

        _authService.Received(1).Authenticate(authenticationRequestDto);

        _tokenService.Received(1).AppendTokens(userDto);
    }

    [Fact]
    public void Authenticate_WhenAuthPasses_ReturnsUnauthorizedResult()
    {
        //Arrange
        var authenticationRequestViewModel = _fixture.Build<AuthenticationRequestViewModel>().Create();
        var authenticationRequestDto = _fixture.Build<AuthenticationRequestDto>().Create();

        _mapper.Map<AuthenticationRequestDto>(authenticationRequestViewModel).Returns(authenticationRequestDto);

        _authService.Authenticate(authenticationRequestDto).ReturnsNull();

        var controller = GetController();

        // Act
        var actionResult = controller.Authenticate(authenticationRequestViewModel);

        // Assert

        _mapper.Received(1).Map<AuthenticationRequestDto>(authenticationRequestViewModel);

        _authService.Received(1).Authenticate(authenticationRequestDto);

        var result = actionResult.Result;
        result?.AssertResult<UnauthorizedResult>();

    }

    [Fact]
    public void Refresh_WhenValidLoggedInUser_MapsAndReturnsAuthenticationResponseViewModel()
    {
        // Arrange
        var userDto = _fixture.Build<UserDto>().Create();
        _authorizedUserProvider.GetLoggedInUser().Returns(userDto);

        var authenticationResponseDto = _fixture.Build<AuthenticationResponseDto>().Create();
        _tokenService.AppendTokens(userDto).Returns(authenticationResponseDto);

        var authenticationResponseViewModel = _fixture.Build<AuthenticationResponseViewModel>().Create();
        _mapper.Map<AuthenticationResponseViewModel>(authenticationResponseDto)
            .Returns(authenticationResponseViewModel);

        var controller = GetController();

        // Act
        var actionResult = controller.Refresh();

        // Assert
        var result = actionResult.AssertObjectResult<AuthenticationResponseViewModel, OkObjectResult>();
        result.Should().BeSameAs(authenticationResponseViewModel);

        _authorizedUserProvider.Received(1).GetLoggedInUser();

        _tokenService.Received(1).AppendTokens(userDto);

        _mapper.Received(1).Map<AuthenticationResponseViewModel>(authenticationResponseDto);
    }

    [Fact]
    public void Refresh_WhenNoLoggedInUserFound_ReturnsUnauthorizedResult()
    {
        // Arrange
        _authorizedUserProvider.GetLoggedInUser().ReturnsNull();

        var controller = GetController();

        // Act
        var actionResult = controller.Refresh();

        // Assert
        _authorizedUserProvider.Received(1).GetLoggedInUser();

        var result = actionResult.Result;
        result?.AssertResult<UnauthorizedResult>();
    }

    private AuthenticationController GetController()
    {
        return new AuthenticationController(_authService, _tokenService, _mapper, _authorizedUserProvider);
    }
}