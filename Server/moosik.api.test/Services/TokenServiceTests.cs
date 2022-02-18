using System.IdentityModel.Tokens.Jwt;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using moosik.api.Authentication.Services;
using moosik.services.Dtos;
using moosik.services.Dtos.Authentication;
using NSubstitute;
using Xunit;

namespace moosik.api.test.Services;

public class TokenServiceTests
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public TokenServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        _configuration = Substitute.For<IConfiguration>();
        _mapper = Substitute.For<IMapper>();
    }

    [Theory]
    [InlineData("User")]
    [InlineData("Admin")]
    [InlineData("SuperAdmin")]
    public void GenerateToken_WhenValidArgsReceived_ValidTokenReturned(string role)
    {
        // Arrange
        const int timeInMinutes = 10;
        _configuration["JWT:Key"].Returns("This is my supper secret key for jwt");

        var authenticationResponseDto = _fixture.Build<AuthenticationResponseDto>()
            .With(x => x.Role, role)
            .Create();

        var service = GetService();

        // Act
        var result = service.GenerateToken(authenticationResponseDto, timeInMinutes);

        // Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        
        Assert.True(tokenHandler.CanReadToken(result));

    }

    [Fact]
    public void AppendTokens_WhenValidArgsReceived_TokensAppended()
    {
        // Arrange
        var accessToken = _fixture.Create<string>();
        var refreshToken = _fixture.Create<string>();
        _configuration["JWT:Key"].Returns("This is my supper secret key for jwt");
        
        
        var userDto = _fixture.Build<UserDto>().Create();
        var authenticationResponseDto = _fixture.Build<AuthenticationResponseDto>()
            .With(x => x.AccessToken, accessToken)
            .With(x => x.RefreshToken, refreshToken)
            .Create();

        _mapper.Map<AuthenticationResponseDto>(userDto).Returns(authenticationResponseDto);
        
        var service = GetService();
        service.GenerateToken(authenticationResponseDto, 30).Returns(accessToken);
        service.GenerateToken(authenticationResponseDto, 20160).Returns(refreshToken);
        
        // Act
        var result = service.AppendTokens(userDto);

        // Assert
        result.Should().BeEquivalentTo(authenticationResponseDto);
    }
    
    private TokenService GetService()
    {
        return new TokenService(_configuration, _mapper);
    }
}