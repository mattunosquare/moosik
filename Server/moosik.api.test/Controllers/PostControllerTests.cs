using System;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using moosik.api.Controllers;
using moosik.api.test.Extensions;
using moosik.api.ViewModels;
using moosik.services.Dtos;
using moosik.services.Interfaces;
using NSubstitute;
using Xunit;

namespace moosik.api.test.Controllers;

public class PostControllerTests
{
    private readonly IPostService _service;
    private readonly IMapper _mapper;
    
    public PostControllerTests()
    {
        _service = Substitute.For<IPostService>();
        _mapper = Substitute.For<IMapper>();
    }

    [Fact]
    public void GetAllPost_WhenCalledWithNoArgs_MapsAndReturnAllPosts()
    {
        // Arrange
        var postsDto = new []
        {
            new PostDto()
        };
        _service.GetAllPosts().Returns(postsDto);
        var postsViewModel = new []
        {
            new PostViewModel()
        };
        _mapper.Map<PostViewModel[]>(postsDto).Returns(postsViewModel);
        var controller = GetController();
   
        // Act
        var actionResult = controller.GetAllPost();
        
        // Assert
        var result = actionResult.AssertObjectResult<PostViewModel[], OkObjectResult>();
        result.Should().BeSameAs(postsViewModel);
        
        _mapper.Received().Map<PostViewModel[]>(postsDto);
        _service.Received().GetAllPosts();

    }

    [Fact]
    public void GetAllPost_WhenCalledWithThreadIdAsParam_MapsAndReturnPostsWithMatchingThreadId()
    {
        //Arrange
        const int threadId = 1;

        var postsDto = new[]
        {
            new PostDto()
            {
                ThreadId = threadId
            }
        };
        _service.GetAllPosts(threadId).Returns(postsDto);

        var postsViewModel = new[]
        {
            new PostViewModel()
            {
                ThreadId = threadId
            }
        };
        _mapper.Map<PostViewModel[]>(postsDto).Returns(postsViewModel);
        
        var controller = GetController();
        
        //Act
        var actionResult = controller.GetAllPost(threadId);

        //Assert
        var result = actionResult.AssertObjectResult<PostViewModel[], OkObjectResult>();
        result.Should().BeSameAs(postsViewModel);
        
        _mapper.Received().Map<PostViewModel[]>(postsDto);
        _service.Received().GetAllPosts(threadId);
    }

    [Fact]
    public void GetAllPost_WhenNoPostsFound_ReturnsEmptyArray()
    {
        //Arrange
        var postsDto = Array.Empty<PostDto>();
        _service.GetAllPosts().Returns(postsDto);

        var controller = GetController();
        
        //Act
        var actionResult = controller.GetAllPost();
        
        //Assert
        actionResult.AssertResult<PostViewModel[], NoContentResult>();

        _mapper.Received().Map<PostViewModel[]>(postsDto);
    }

    [Fact]
    public void GetPostById_WhenPostFound_MappedAndReturned()
    {
        //Arrange
        const int postId = 1;
        var postDto = new PostDto() {Id = postId};
        _service.GetPostById(postId).Returns(postDto);

        var postViewModel = new PostViewModel() {Id = postId};
        _mapper.Map<PostViewModel>(postDto).Returns(postViewModel);

        var controller = GetController();
        
        //Act
        var actionResult = controller.GetPostById(postId);

        //Assert
        var result = actionResult.AssertObjectResult<PostViewModel, OkObjectResult>();
        result.Should().BeSameAs(postViewModel);

        _mapper.Received().Map<PostViewModel>(postDto);
    }

    [Fact]
    public void GetPostById_WhenNoPostFound_ReturnsNotFound()
    {
        // Arrange
        var controller = GetController();

        // Act
        var actionResult = controller.GetPostById(1);

        // Assert
        actionResult.AssertResult<PostViewModel, NotFoundResult>();
        _mapper.Received(1).Map<PostViewModel>(null);
    }

    [Fact]
    public void GetPostsAfterDate_WhenPostsFound_MappedAndReturned()
    {
        // Arrange
        var createdDate = DateTime.UtcNow;

        var postsDto = new[]
        {
            new PostDto() {CreatedDate = createdDate.AddMinutes(30)}
        };
        _service.GetPostsAfterDate(createdDate).Returns(postsDto);

        var postsViewModel = new[]
        {
            new PostViewModel() {CreatedDate = createdDate.AddMinutes(30)}
        };
        _mapper.Map<PostViewModel[]>(postsDto).Returns(postsViewModel);

        var controller = GetController();

        // Act
        var actionResult = controller.GetPostsAfterDate(createdDate);

        // Assert
        var result = actionResult.AssertObjectResult<PostViewModel[], OkObjectResult>();
        result.Should().BeSameAs(postsViewModel);
        
        _mapper.Received().Map<PostViewModel[]>(postsDto);
        _service.Received().GetPostsAfterDate(createdDate);
    }

    [Fact]
    public void GetPostsAfterDate_WhenNoPostsFound_ReturnsEmptyArray()
    {
        // Arrange
        var date = DateTime.UtcNow;
        var postDto = Array.Empty<PostDto>();
        _service.GetPostsAfterDate(date).Returns(postDto);

        var controller = GetController();
        
        // Act
        var actionResult = controller.GetPostsAfterDate(date);

        // Assert
        actionResult.AssertResult<PostViewModel[], NoContentResult>();
        _mapper.Received().Map<PostViewModel[]>(postDto);
    }

    [Fact]
    public void AddPost_WhenAddSuccessful_ReturnsNoContent()
    {
        // Arrange
        const int threadId = 1;
        var createPostDto = new CreatePostDto();

        var createPostViewModel = new CreatePostViewModel();
        _mapper.Map<CreatePostDto>(createPostViewModel).Returns(createPostDto);

        var controller = GetController();

        // Act
        var actionResult = controller.CreatePost(createPostViewModel,threadId);

        // Assert
        actionResult.AssertResult<NoContentResult>();
        _mapper.Received(1).Map<CreatePostDto>(createPostViewModel);
        _service.Received(1).CreatePost(Arg.Is<CreatePostDto>(x => x == createPostDto && x.ThreadId == threadId));
    }
    
    [Fact]
    public void UpdatePost_WhenPostPassedWithRequiredFields_ReturnsNoContent()
    {
        // Arrange
        const int postId = 1;

        var updatePostViewModel = new UpdatePostViewModel();

        var updatePostDto = new UpdatePostDto();
        _mapper.Map<UpdatePostDto>(updatePostViewModel).Returns(updatePostDto);
        
        var controller = GetController();
        
        // Act
        var actionResult = controller.UpdatePost(postId, updatePostViewModel);

        // Assert
        actionResult.AssertResult<NoContentResult>();

        _mapper.Received(1).Map<UpdatePostDto>(updatePostViewModel);
        _service.Received(1).UpdatePost(Arg.Is<UpdatePostDto>(x => x == updatePostDto && x.Id == postId));
    }

    [Fact]
    public void DeletePost_WhenDeleteSuccessful_ReturnsNoContent()
    {
        // Arrange
        const int postId = 1;

        var controller = GetController();
        
        // Act
        var actionResult = controller.DeletePost(postId);

        // Assert
        actionResult.AssertResult<NoContentResult>();
        _service.Received(1).DeletePost(postId);
    }

    [Fact]
    public void UpdatePostResource_WhenResourcePassedWithRequiredFields_ReturnsNoContent()
    {
        // Arrange
        const int postResourceId = 1;

        var updatePostResourceViewModel = new UpdatePostResourceViewModel();

        var updatePostResourceDto = new UpdatePostResourceDto();
        _mapper.Map<UpdatePostResourceDto>(updatePostResourceViewModel).Returns(updatePostResourceDto);

        var controller = GetController();

        // Act
        var actionResult = controller.UpdatePostResource(postResourceId, updatePostResourceViewModel);

        // Assert
        actionResult.AssertResult<NoContentResult>();

        _mapper.Received(1).Map<UpdatePostResourceDto>(updatePostResourceViewModel);
        _service.Received(1).UpdatePostResource(Arg.Is<UpdatePostResourceDto>(x => x == updatePostResourceDto && x.Id == postResourceId));
    }

    [Fact]
    public void GetAllResourceTypes_WhenResourceTypesFound_MappedAndReturned()
    {
        // Arrange
        var resourceTypesDto = new[]
        {
            new ResourceTypeDto()
        };
        _service.GetAllResourceTypes().Returns(resourceTypesDto);

        var resourceTypesViewModel = new[]
        {
            new ResourceTypeViewModel()
        };
        _mapper.Map<ResourceTypeViewModel[]>(resourceTypesDto).Returns(resourceTypesViewModel);

        var controller = GetController();
        
        // Act
        var actionResult = controller.GetAllResourceTypes();

        // Assert
        var result = actionResult.AssertObjectResult<ResourceTypeViewModel[], OkObjectResult>();
        result.Should().BeSameAs(resourceTypesViewModel);

        _mapper.Received().Map<ResourceTypeViewModel[]>(resourceTypesDto);
        _service.Received(1).GetAllResourceTypes();
    }

    [Fact]
    public void GetAllResourceTypes_WhenNoResourcesFound_ReturnsEmptyArray()
    {
        // Arrange
        var resourceTypesDto = Array.Empty<ResourceTypeDto>();
        _service.GetAllResourceTypes().Returns(resourceTypesDto);

        var controller = GetController();

        // Act
        var actionResult = controller.GetAllResourceTypes();

        // Assert
        actionResult.AssertResult<ResourceTypeViewModel[], NoContentResult>();
        _mapper.Received().Map<ResourceTypeViewModel[]>(resourceTypesDto);
    }
    private PostController GetController()
    {
        return new PostController(_service, _mapper);
    }
}