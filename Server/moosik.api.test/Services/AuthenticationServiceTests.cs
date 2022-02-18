using System.Linq;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using moosik.api.Authentication.Services;
using moosik.dal.Interfaces;
using moosik.dal.Models;
using moosik.services.Dtos;
using moosik.services.Dtos.Authentication;
using NSubstitute;
using Xunit;
using BC = BCrypt.Net.BCrypt;

namespace moosik.api.test.Services;

public class AuthenticationServiceTests
{
    private readonly IMoosikDatabase _database;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public AuthenticationServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        _database = Substitute.For<IMoosikDatabase>();
        _mapper = Substitute.For<IMapper>();
    }

    [Fact]
    public void Authenticate_WhenValidArgsPassed_MapsAndReturnsUserDto()
    {
        // Arrange
        const string passwordString = "password";
        var salt = BC.GenerateSalt(6);
        var password = BC.HashPassword(passwordString, salt);
        
        var authenticationRequestDto = _fixture.Build<AuthenticationRequestDto>()
            .With(x => x.Password, passwordString)
            .Create();
        
        var filteredUserQueryable = _fixture.Build<User>()
            .With(x => x.Username, authenticationRequestDto.Username)
            .With(x => x.Password, password)
            .CreateMany(1).AsQueryable();
        
        var allUsersQueryable = _fixture.Build<User>().CreateMany(10).Concat(filteredUserQueryable).AsQueryable();
        _database.Get<User>().Returns(allUsersQueryable);
        
        var filteredUser = filteredUserQueryable.SingleOrDefault();
        var expectedResult = _fixture.Build<UserDto>().Create();

        _mapper.Map<UserDto>(Arg.Any<User>()).Returns(expectedResult);
        var service = GetService();

        // Act
        var result = service.Authenticate(authenticationRequestDto);

        // Assert
        _database.Received(1).Get<User>();

        _mapper.Received(1).Map<UserDto>(Arg.Is<User>(
            input => input.Equals(filteredUser)));

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Authenticate_IsAuthFails_ReturnsNull()
    {
        // Arrange
        const string passwordString = "password";
        const string incorrectPasswordString = "notPassword";
        var salt = BC.GenerateSalt(6);
        var password = BC.HashPassword(passwordString, salt);
        
        var authenticationRequestDto = _fixture.Build<AuthenticationRequestDto>()
            .With(x => x.Password, incorrectPasswordString)
            .Create();
        
        var filteredUserQueryable = _fixture.Build<User>()
            .With(x => x.Username, authenticationRequestDto.Username)
            .With(x => x.Password, password)
            .CreateMany(1).AsQueryable();
        
        var allUsersQueryable = _fixture.Build<User>().CreateMany(10).Concat(filteredUserQueryable).AsQueryable();
        _database.Get<User>().Returns(allUsersQueryable);

        var service = GetService();

        // Act
        var result = service.Authenticate(authenticationRequestDto);

        // Assert
        _database.Received(1).Get<User>();

        result.Should().BeNull();
    }
    
    private AuthenticationService GetService()
    {
        return new AuthenticationService(_database, _mapper);
    }
}