using System;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using moosik.api.Controllers;
using moosik.api.test.Extensions;
using moosik.api.ViewModels.User;
using moosik.services.Dtos;
using moosik.services.Dtos.User;
using moosik.services.Interfaces;
using NSubstitute;
using Xunit;

namespace moosik.api.test.Controllers;

public class UserControllerTests
{
    private readonly IUserService _service;
    private readonly IMapper _mapper;

    public UserControllerTests()
    {
        _service = Substitute.For<IUserService>();
        _mapper = Substitute.For<IMapper>();
    }

    [Fact]
    public void GetAllUsers_WhenCalledWithNoArgs_MapsAndReturnsAllUsers()
    {
        // Arrange
        var usersDto = new[]
        {
            new UserDto()
        };
        _service.GetAllUsers().Returns(usersDto);

        var usersViewModel = new[]
        {
            new UserViewModel()
        };
        _mapper.Map<UserViewModel[]>(usersDto).Returns(usersViewModel);

        var controller = GetController();

        // Act
        var actionResult = controller.GetAllUsers();

        // Assert
        var result = actionResult.AssertObjectResult<UserViewModel[], OkObjectResult>();
        result.Should().BeSameAs(usersViewModel);

        _mapper.Received().Map<UserViewModel[]>(usersDto);
        _service.Received().GetAllUsers();
    }

    [Fact]
    public void GetAllUsers_WhenCalledWithUserIdAsParam_MapsAndReturnsUserMatchingUserId()
    {
        // Arrange
        const int userId = 1;

        var usersDto = new []
        {
            new UserDto() {Id = userId}
        };
        _service.GetAllUsers(userId).Returns(usersDto);

        var usersViewModel = new[]
        {
            new UserViewModel() {Id = userId}
        };
        _mapper.Map<UserViewModel[]>(usersDto).Returns(usersViewModel);

        var controller = GetController();

        // Act
        var actionResult = controller.GetAllUsers(userId);

        // Assert
        var result = actionResult.AssertObjectResult<UserViewModel[], OkObjectResult>();
        result.Should().BeSameAs(usersViewModel);

        _mapper.Received().Map<UserViewModel[]>(usersDto);
        _service.Received().GetAllUsers(userId);
    }

    [Fact]
    public void GetAllUsers_WhenNoUsersFound_ReturnsEmptyArray()
    {
        // Arrange
        var usersDto = Array.Empty<UserDto>();
        _service.GetAllUsers().Returns(usersDto);

        var controller = GetController();

        // Act
        var actionResult = controller.GetAllUsers();

        // Assert
        actionResult.AssertResult<UserViewModel[], NoContentResult>();

        _mapper.Received().Map<UserViewModel[]>(usersDto);
    }

    [Fact]
    public void GetDetailedUserById_WhenUserFound_MappedAndReturned()
    {
        // Arrange
        const int userId = 1;
        var detailedUserDto = new UserDetailDto() {Id = userId};
        _service.GetDetailedUserById(userId).Returns(detailedUserDto);

        var detailedUserViewModel = new UserDetailViewModel() {Id = userId};
        _mapper.Map<UserDetailViewModel>(detailedUserDto).Returns(detailedUserViewModel);

        var controller = GetController();

        // Act
        var actionResult = controller.GetDetailedUserById(userId);

        // Assert
        var result = actionResult.AssertObjectResult<UserDetailViewModel, OkObjectResult>();
        result.Should().BeSameAs(detailedUserViewModel);

        _mapper.Received().Map<UserDetailViewModel>(detailedUserDto);
    }

    [Fact]
    public void GetDetailedUserById_WhenNoUserFound_ReturnsNotFound()
    {
        // Arrange
        var controller = GetController();

        // Act
        var actionResult = controller.GetDetailedUserById(1);

        // Assert
        actionResult.AssertResult<UserDetailViewModel, NotFoundResult>();
        _mapper.Received(1).Map<UserDetailViewModel>(null);
    }

    [Fact]
    public void GetUserByUsernameAndEmail_WhenUserFound_MappedAndReturned()
    {
        const string username = "Username1";
        const string email = "username@email.com";

        // Arrange
        var userDto = new UserDto()
        {
            Username = username,
            Email = email
        };
        _service.GetUserByUsernameAndEmail(username, email).Returns(userDto);

        var userViewModel = new UserViewModel()
        {
            Username = username,
            Email = email
        };
        _mapper.Map<UserViewModel>(userDto).Returns(userViewModel);

        var controller = GetController();

        // Act
        var actionResult = controller.GetUserByUsernameAndEmail(username, email);

        // Assert
        var result = actionResult.AssertObjectResult<UserViewModel, OkObjectResult>();
        result.Should().BeSameAs(userViewModel);

        _mapper.Received().Map<UserViewModel>(userDto);
        _service.Received().GetUserByUsernameAndEmail(username, email);
    }

    [Fact]
    public void GetUserByUsernameAndEmail_WhenNoUserFound_ReturnsNotFound()
    {
        // Arrange
        const string username = "Username1";
        const string email = "username@email.com";
        
        var controller = GetController();

        // Act
        var actionResult = controller.GetUserByUsernameAndEmail(username, email);

        // Assert
        actionResult.AssertResult<UserViewModel, NotFoundResult>();
        _mapper.Received(1).Map<UserViewModel>(null);
    }

    [Fact]
    public void CreateUser_WhenAddSuccessful_ReturnsNoContent()
    {
        // Arrange
        var createUserDto = new CreateUserDto();

        var createUserViewModel = new CreateUserViewModel();
        _mapper.Map<CreateUserDto>(createUserViewModel).Returns(createUserDto);

        var controller = GetController();
        
        // Act
        var actionResult = controller.CreateUser(createUserViewModel);
        
        // Assert
        actionResult.AssertResult<NoContentResult>();
    }

    [Fact]
    public void UpdateUser_WhenUserPassedWithRequiredFields_ReturnsNoContent()
    {
        // Arrange
        const int userId = 1;

        var updateUserViewModel = new UpdateUserViewModel();

        var updateUserDto = new UpdateUserDto();
        _mapper.Map<UpdateUserDto>(updateUserViewModel).Returns(updateUserDto);

        var controller = GetController();

        // Act
        var actionResult = controller.UpdateUser(userId, updateUserViewModel);

        // Assert
        actionResult.AssertResult<NoContentResult>();

        _mapper.Received(1).Map<UpdateUserDto>(updateUserViewModel);
        _service.Received(1).UpdateUser(Arg.Is<UpdateUserDto>(x => x == updateUserDto && x.Id == userId));
    }

    [Fact]
    public void DeleteUser_WhenDeleteSuccessful_ReturnsNoContent()
    {
        // Arrange
        const int userId = 1;

        var controller = GetController();
        
        // Act
        var actionResult = controller.DeleteUser(userId);

        // Assert
        actionResult.AssertResult<NoContentResult>();
        _service.Received(1).DeleteUser(userId);
    }
    private UserController GetController()
    {
        return new UserController(_service, _mapper);
    }
}