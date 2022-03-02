using System;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using moosik.api.Controllers;
using moosik.api.test.Extensions;
using moosik.api.ViewModels;
using moosik.api.ViewModels.Thread;
using moosik.api.ViewModels.User;
using moosik.services.Dtos;
using moosik.services.Dtos.Thread;
using moosik.services.Interfaces;
using NSubstitute;
using Xunit;

namespace moosik.api.test.Controllers;

public class ThreadControllerTests
{
    private readonly IThreadService _service;
    private readonly IMapper _mapper;

    public ThreadControllerTests()
    {
        _service = Substitute.For<IThreadService>();
        _mapper = Substitute.For<IMapper>();
    }

    [Fact]
    public void GetAllThreads_WhenCalledWithNoArgs_MapsAndReturnAllThreads()
    {
        // Arrange
        var threadsDto = new[]
        {
            new ThreadDto()
        };
        _service.GetAllThreads().Returns(threadsDto);

        var threadsViewModel = new[]
        {
            new ThreadViewModel()
        };
        _mapper.Map<ThreadViewModel[]>(threadsDto).Returns(threadsViewModel);

        var controller = GetController();

        // Act
        var actionResult = controller.GetAllThreads();

        // Assert
        var result = actionResult.AssertObjectResult<ThreadViewModel[], OkObjectResult>();
        result.Should().BeSameAs(threadsViewModel);

        _mapper.Received().Map<ThreadViewModel[]>(threadsDto);
        _service.Received().GetAllThreads();
    }

    [Fact]
    public void GetAllThreads_WhenCalledWithUserIdAsParam_MapsAndReturnThreadWithMatchingUserId()
    {
        // Arrange
        const int userId = 1;

        var threadsDto = new[]
        {
            new ThreadDto()
            {
                User = new UserDto()
                {
                    Id = userId
                }
            }
        };
        _service.GetAllThreads(userId).Returns(threadsDto);

        var threadsViewModel = new[]
        {
            new ThreadViewModel()
            {
                User = new UserViewModel()
                {
                    Id = userId
                }
            }
        };
        _mapper.Map<ThreadViewModel[]>(threadsDto).Returns(threadsViewModel);

        var controller = GetController();

        // Act
        var actionResult = controller.GetAllThreads(userId);

        // Assert
        var result = actionResult.AssertObjectResult<ThreadViewModel[], OkObjectResult>();
        result.Should().BeSameAs(threadsViewModel);

        _mapper.Received().Map<ThreadViewModel[]>(threadsDto);
        _service.Received().GetAllThreads(userId);
    }

    [Fact]
    public void GetAllThreads_WhenNoThreadsFound_ReturnsEmptyArray()
    {
        // Arrange
        var threadsDto = Array.Empty<ThreadDto>();
        _service.GetAllThreads().Returns(threadsDto);

        var controller = GetController();

        // Act
        var actionResult = controller.GetAllThreads();

        // Assert
        actionResult.AssertResult<ThreadViewModel[], NoContentResult>();

        _mapper.Received().Map<ThreadViewModel[]>(threadsDto);
    }

    [Fact]
    public void GetThreadById_WhenThreadFound_MapperAndReturned()
    {
        // Arrange
        const int threadId = 1;
        var threadDto = new ThreadDto() {Id = threadId};
        _service.GetThreadById(threadId).Returns(threadDto);

        var threadViewModel = new ThreadViewModel() {Id = threadId};
        _mapper.Map<ThreadViewModel>(threadDto).Returns(threadViewModel);

        var controller = GetController();

        // Act
        var actionResult = controller.GetThreadById(threadId);

        // Assert
        var result = actionResult.AssertObjectResult<ThreadViewModel, OkObjectResult>();
        result.Should().BeSameAs(threadViewModel);

        _mapper.Received(1).Map<ThreadViewModel>(threadDto);
        _service.Received(1).GetThreadById(threadId);
    }

    [Fact]
    public void GetThreadById_WhenNoThreadFound_ReturnsNotFound()
    {
        // Arrange
        var controller = GetController();

        // Act
        var actionResult = controller.GetThreadById(1);

        // Assert
        actionResult.AssertResult<ThreadViewModel, NotFoundResult>();
        _service.Received(1).GetThreadById(1);
        _mapper.Received(1).Map<ThreadViewModel>(null);
    }

    [Fact]
    public void GetThreadsAfterDate_WhenThreadsFound_MappedAndReturned()
    {
        // Arrange
        var createdDate = DateTime.UtcNow;

        var threadsDto = new[]
        {
            new ThreadDto() {CreatedDate = createdDate.AddMinutes(30)}
        };
        _service.GetThreadsAfterDate(createdDate).Returns(threadsDto);

        var threadsViewModel = new[]
        {
            new ThreadViewModel() {CreatedDate = createdDate.AddMinutes(30)}
        };
        _mapper.Map<ThreadViewModel[]>(threadsDto).Returns(threadsViewModel);

        var controller = GetController();
        
        // Act
        var actionResult = controller.GetThreadsAfterDate(createdDate);

        // Assert
        var result = actionResult.AssertObjectResult<ThreadViewModel[], OkObjectResult>();
        result.Should().BeSameAs(threadsViewModel);

        _mapper.Received().Map<ThreadViewModel[]>(threadsDto);
        _service.Received(1).GetThreadsAfterDate(createdDate);
    }

    [Fact]
    public void GetThreadsAfterDate_WhenNoThreadsFound_ReturnsEmptyArray()
    {
        // Arrange
        var date = DateTime.UtcNow;
        var threadsDto = Array.Empty<ThreadDto>();
        _service.GetThreadsAfterDate(date).Returns(threadsDto);

        var controller = GetController();

        // Act
        var actionResult = controller.GetThreadsAfterDate(date);

        // Assert
        actionResult.AssertResult<ThreadViewModel[], NoContentResult>();
        _mapper.Received().Map<ThreadViewModel[]>(threadsDto);
    }

    [Fact]
    public void CreateThread_WhenCreateSuccessful_ReturnNoContent()
    {
        // Arrange
        var createThreadDto = new CreateThreadDto();

        var createThreadViewModel = new CreateThreadViewModel();
        _mapper.Map<CreateThreadDto>(createThreadViewModel).Returns(createThreadDto);

        var controller = GetController();

        // Act
        var actionResult = controller.CreateThread(createThreadViewModel);

        // Assert
        actionResult.AssertResult<NoContentResult>();
    }

    [Fact]
    public void UpdateThread_WhenThreadPassedWithRequiredFields_ReturnsNoContent()
    {
        // Arrange
        const int threadId = 1;

        var updateThreadViewModel = new UpdateThreadViewModel();

        var updateThreadDto = new UpdateThreadDto();
        _mapper.Map<UpdateThreadDto>(updateThreadViewModel).Returns(updateThreadDto);

        var controller = GetController();
        
        // Act
        var actionResult = controller.UpdateThread(threadId, updateThreadViewModel);

        // Assert
        actionResult.AssertResult<NoContentResult>();

        _mapper.Received(1).Map<UpdateThreadDto>(updateThreadViewModel);
        _service.Received(1).UpdateThread(Arg.Is<UpdateThreadDto>(x => x == updateThreadDto && x.Id == threadId));
    }

    [Fact]
    public void DeleteThread_WhenDeleteSuccessful_ReturnsNoContent()
    {
        // Arrange
        const int threadId = 1;

        var controller = GetController();

        // Act
        var actionResult = controller.DeleteThread(threadId);

        // Assert
        actionResult.AssertResult<NoContentResult>();
        _service.Received(1).DeleteThread(threadId);
    }

    [Fact]
    public void GetAllThreadTypes_WhenThreadTypesFound_MappedAndReturned()
    {
        // Arrange
        var threadTypesDto = new[]
        {
            new ThreadTypeDto()
        };
        _service.GetAllThreadTypes().Returns(threadTypesDto);

        var threadTypesViewModel = new[]
        {
            new ThreadTypeViewModel()
        };
        _mapper.Map<ThreadTypeViewModel[]>(threadTypesDto).Returns(threadTypesViewModel);

        var controller = GetController();
        
        // Act
        var actionResult = controller.GetAllThreadTypes();

        // Assert
        var result = actionResult.AssertObjectResult<ThreadTypeViewModel[], OkObjectResult>();
        result.Should().BeSameAs(threadTypesViewModel);

        _mapper.Received().Map<ThreadTypeViewModel[]>(threadTypesDto);
        _service.Received(1).GetAllThreadTypes();
    }

    [Fact]
    public void GetAllThreadTypes_WhenNoThreadTypesFound_ReturnsEmptyArray()
    {
        // Arrange
        var threadTypesDto = Array.Empty<ThreadTypeDto>();
        _service.GetAllThreadTypes().Returns(threadTypesDto);

        var controller = GetController();

        // Act
        var actionResult = controller.GetAllThreadTypes();

        // Assert
        actionResult.AssertResult<ThreadTypeViewModel[], NoContentResult>();
        _mapper.Received().Map<ThreadTypeViewModel[]>(threadTypesDto);
    }

    private ThreadController GetController()
    {
        return new ThreadController(_service, _mapper);
    }
}