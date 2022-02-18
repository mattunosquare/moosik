using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using moosik.dal.Interfaces;
using moosik.dal.Models;
using moosik.services.Dtos;
using moosik.services.Dtos.User;
using moosik.services.Exceptions;
using moosik.services.Services;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace moosik.services.test.Services;

public class UserServiceTests
{
    private readonly IMoosikDatabase _database;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public UserServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        _database = Substitute.For<IMoosikDatabase>();
        _mapper = Substitute.For<IMapper>();
    }

    [Fact]
    public void GetAllUsers_WhenCalledWithNoArgs_MapsAndReturnsAll()
    {
        // Arrange
        var usersQueryable = _fixture.Build<User>().CreateMany(10).AsQueryable();
        _database.Get<User>().Returns(usersQueryable);

        var usersDtoQueryable = _fixture.Build<UserDto>().CreateMany(10).AsQueryable();
        _mapper.ProjectTo<UserDto>(Arg.Any<IQueryable<User>>()).Returns(usersDtoQueryable);

        var expectedResult = usersDtoQueryable.ToArray();
        
        var service = GetService();

        // Act
        var result = service.GetAllUsers();

        // Assert
        _database.Received(1).Get<User>();

        _mapper.Received(1).ProjectTo<UserDto>(Arg.Is<IQueryable<User>>(
            input => input.SequenceEqual(usersQueryable)));

        result.Should().BeEquivalentTo(expectedResult);

    }

    [Fact]
    public void GetAllUsers_WhenCalledWithUserId_MapsAndReturnsUsersWithMatchingId()
    {
        // Arrange
        const int userId = 1;
        var filteredUsersQueryable = _fixture.Build<User>().With(x => x.Id, userId).CreateMany(1).AsQueryable();
        var allUsersQueryable = _fixture.Build<User>().CreateMany(10).Concat(filteredUsersQueryable).AsQueryable();

        _database.Get<User>().Returns(allUsersQueryable);

        var userDtoQueryable = _fixture.Build<UserDto>().With(x => x.Id, userId).CreateMany(1).AsQueryable();

        _mapper.ProjectTo<UserDto>(Arg.Any<IQueryable<User>>()).Returns(userDtoQueryable);

        var expectedResult = userDtoQueryable.ToArray();

        var service = GetService();

        // Act
        var result = service.GetAllUsers(userId);

        // Assert

        _database.Received(1).Get<User>();

        _mapper.Received(1).ProjectTo<UserDto>(Arg.Is<IQueryable<User>>(
            input => input.SequenceEqual(filteredUsersQueryable)));

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void GetAllUsers_WhenNoUsersFound_ReturnsEmptyArray()
    {
        // Arrange
        var expectedResult = Array.Empty<UserDto>();
        var service = GetService();

        // Act
        var result = service.GetAllUsers();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void GetDetailedUserById_WhenUserFound_ReturnsDetailedUser()
    {
        // Arrange
        const int userId = 1;
        var filteredUsersQueryable = _fixture.Build<User>().With(x => x.Id, userId).CreateMany(1).AsQueryable();
        var allUsersQueryable = _fixture.Build<User>().CreateMany(10).Concat(filteredUsersQueryable).AsQueryable();
        _database.Get<User>().Returns(allUsersQueryable);

        var detailedUserDtoQueryable =
            _fixture.Build<UserDetailDto>().With(x => x.Id, userId).CreateMany(1).AsQueryable();

        _mapper.ProjectTo<UserDetailDto>(Arg.Any<IQueryable<User>>()).Returns(detailedUserDtoQueryable);

        var expectedResult = detailedUserDtoQueryable.SingleOrDefault();

        var service = GetService();

        // Act
        var result = service.GetDetailedUserById(userId);

        // Assert
        _database.Received(1).Get<User>();

        _mapper.Received(1).ProjectTo<UserDetailDto>(Arg.Is<IQueryable<User>>(
            input => input.SequenceEqual(filteredUsersQueryable)));

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void GetDetailedUserById_WhenNoUserFound_ReturnsNullObject()
    {
        // Arrange
        const int userId = 1;

        var service = GetService();
        service.GetDetailedUserById(userId).ReturnsNull();

        // Act
        var result = service.GetDetailedUserById(userId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetUserByUsernameAndEmail_WhenUserFound_ReturnsUserDto()
    {
        // Arrange
        const string username = "username";
        const string email = "email@email.com";
        
        var filteredUsersQueryable = _fixture.Build<User>()
            .With(x => x.Username, username)
            .With(x => x.Email, email)
            .CreateMany(1).AsQueryable();
        
        var allUsersQueryable = _fixture.Build<User>().CreateMany(10).Concat(filteredUsersQueryable).AsQueryable();
        _database.Get<User>().Returns(allUsersQueryable);

        var userDtoQueryable = _fixture.Build<UserDto>()
            .With(x => x.Username, username)
            .With(x => x.Email, email)
            .CreateMany(1).AsQueryable();

        _mapper.ProjectTo<UserDto>(Arg.Any<IQueryable<User>>()).Returns(userDtoQueryable);

        var expectedResult = userDtoQueryable.SingleOrDefault();
        
        var service = GetService();

        // Act
        var result = service.GetUserByUsernameAndEmail(username, email);

        // Assert
        _database.Received(1).Get<User>();

        _mapper.Received(1).ProjectTo<UserDto>(Arg.Is<IQueryable<User>>(
            input => input.SequenceEqual(filteredUsersQueryable)));

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void GetUserByUsernameAndEmail_WhenNoUserFound_ReturnsNullObject()
    {
        // Arrange
        const string username = "username";
        const string email = "email@email.com";

        var service = GetService();

        // Act
        var result = service.GetUserByUsernameAndEmail(username, email);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void UpdateUser_IfUserExists_UpdateUserWithNewValues()
    {
        // Arrange
        const int userId = 1;
        var newUsername = _fixture.Create<string>();

        var existingDomainUser = _fixture.Build<User>().With(x => x.Id, userId).CreateMany(1).AsQueryable();
        
        var updateUserDto = _fixture.Build<UpdateUserDto>()
            .With(x => x.Id, userId)
            .With(x => x.Username, newUsername)
            .Create();

        _database.Get<User>().Returns(existingDomainUser);
        
        _mapper.When(x => x.Map(updateUserDto, existingDomainUser)).Do(x =>
        {
            existingDomainUser.ElementAt(0).Username = newUsername;
        });

        var service = GetService();

        // Act
        service.UpdateUser(updateUserDto);

        // Assert
        _database.When(x => x.SaveChanges()).Do(x =>
        {
            existingDomainUser.ElementAt(0).Username.Should().Be(newUsername);
        });
    }

    [Fact]
    public void UpdateUser_IfNoUsrExists_ExceptionThrown()
    {
        // Arrange
        const int userId = 1;

        var updateUserDto = _fixture.Build<UpdateUserDto>().With(x => x.Id, userId).Create();

        var service = GetService();
        
        // Act
        var act = () => service.UpdateUser(updateUserDto);

        // Assert
        act.Should().Throw<NotFoundException>().WithMessage($"No user found for id: {userId}");
    }

    [Fact]
    public void CreateUser_WhenCorrectArgsPassed_CreateNewUser()
    {
        // Arrange
        const int userId = 1;
        var mockDb = new Dictionary<int, User>();

        var createUserDto = _fixture.Build<CreateUserDto>().Create();
        var userDomain = _fixture.Build<User>().With(x => x.Id, userId).Create();

        _mapper.Map<User>(createUserDto).Returns(userDomain);
        
        _database.When(x => x.Add(userDomain)).Do(x =>
        {
            mockDb.Add(userDomain.Id, userDomain);
        });

        var service = GetService();

        // Act
        service.CreateUser(createUserDto);

        // Assert
        _mapper.Received(1).Map<User>(Arg.Is(createUserDto));
        
        _database.When(x => x.SaveChanges()).Do(x =>
        {
            mockDb[userId].Should().BeEquivalentTo(userDomain);
        });
    }

    [Fact]
    public void DeleteUser_WhenUserExists_DeleteUser()
    {
        // Arrange
        const int userId = 1;
        
        var userQueryable = _fixture.Build<User>()
            .With(x => x.Id, userId)
            .With(x => x.Active, true)
            .CreateMany(1)
            .AsQueryable();

        _database.Get<User>().Returns(userQueryable);

        var service = GetService();

        // Act
        service.DeleteUser(userId);

        // Assert
        _database.When(x => x.SaveChanges()).Do(x =>
        {
            userQueryable.ElementAt(0).Active.Should().Be(false);
        });
    }

    [Fact]
    public void DeleteUser_WhenUserNotFound_ExceptionThrown()
    {
        // Arrange
        const int userId = 1;

        var service = GetService();
        
        // Act
        var act = () => service.DeleteUser(userId);

        // Assert
        act.Should().Throw<NotFoundException>().WithMessage($"No user found with userId: {userId}");
    }
    
    [Fact]
    public void RetrieveUserForId_WhenUserFound_ReturnsIQueryableOfUser()
    {
        // Arrange
        const int userId = 1;
        var filteredUsersQueryable = _fixture.Build<User>().With(x => x.Id, userId).CreateMany(1).AsQueryable();
        var allUsersQueryable = _fixture.Build<User>().CreateMany(10).Concat(filteredUsersQueryable).AsQueryable();

        _database.Get<User>().Returns(allUsersQueryable);

        var service = GetService();
        
        // Act
        var result = service.RetrieveUserForId(userId);

        // Assert
        _database.Received(1).Get<User>();

        result.Should().BeEquivalentTo(filteredUsersQueryable);
    }

    [Fact]
    public void RetrieveUserForId_WhenNoUserFound_ReturnsEmptyIQueryable()
    {
        // Arrange
        const int userId = 1;

        _database.Get<User>().Returns(Enumerable.Empty<User>().AsQueryable());
        
        var service = GetService();
        
        // Act
        var result = service.RetrieveUserForId(userId);

        // Assert
        _database.Received(1).Get<User>();
        
        result.Should().BeEmpty();
    }

    private UserService GetService()
    {
        return new UserService(_database, _mapper);
    }
}