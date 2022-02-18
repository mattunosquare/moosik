using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoMapper;
using Castle.Core.Internal;
using FluentAssertions;
using moosik.dal.Interfaces;
using moosik.dal.Models;
using moosik.services.Dtos;
using moosik.services.Dtos.Post;
using moosik.services.Exceptions;
using moosik.services.Services;
using NSubstitute;
using Xunit;

namespace moosik.services.test.Services;

public class PostServiceTests

{
    private readonly IMoosikDatabase _database;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public PostServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        _database = Substitute.For<IMoosikDatabase>();
        _mapper = Substitute.For<IMapper>();
    }

    [Fact]
    public void GetAllPosts_WhenCalledWithNoArgs_MapsAndReturnsAll()
    {
        // Arrange
        var posts = _fixture.Build<Post>().CreateMany().AsQueryable();
        _database.Get<Post>().Returns(posts);

        var postsDto = _fixture.CreateMany<PostDto>().AsQueryable();
        _mapper.ProjectTo<PostDto>(Arg.Any<IQueryable<Post>>()).Returns(postsDto);

        var postsDtoArray = postsDto.ToArray();
        
        var service = GetService();
        
        // Act
        var result = service.GetAllPosts();

        // Assert
        _database.Received().Get<Post>();
        _mapper.Received().ProjectTo<PostDto>(Arg.Is<IQueryable<Post>>(
            input => input.SequenceEqual(posts)));
        
        result.Should().BeEquivalentTo(postsDtoArray);
        
    }

    [Fact]
    public void GetAllPosts_WhenCalledWithThreadIdParam_MapsAndReturnsPostsWithMatchingThreadId()
    {
        // Arrange
        const int threadId = 1;
        var filteredPosts  = _fixture.Build<Post>().With(x => x.ThreadId, threadId).CreateMany(10).AsQueryable();
        var postQueryable = _fixture.CreateMany<Post>(10).Concat(filteredPosts).AsQueryable();
        
        _database.Get<Post>().Returns(postQueryable);

        var postDtoQueryable = _fixture.Build<PostDto>().With(x => x.ThreadId, threadId).CreateMany(10).AsQueryable();

        _mapper.ProjectTo<PostDto>(Arg.Any<IQueryable<Post>>())
            .Returns(postDtoQueryable);

        var postsDto = postDtoQueryable.ToArray();

        var service = GetService();

        // Act
        var result = service.GetAllPosts(threadId);

        // Assert
        _database.Received(1).Get<Post>();
        
        _mapper.Received(1).ProjectTo<PostDto>(Arg.Is<IQueryable<Post>>(
            input => input.SequenceEqual(filteredPosts)));
        
        result.Should().BeEquivalentTo(postsDto);
    }

    [Fact]
    public void GetAllPosts_WhenNoPostsFound_ReturnsEmptyArray()
    {
        // Arrange
        var expectedResult = Array.Empty<PostDto>();
        var service = GetService();

        // Act
        var result = service.GetAllPosts();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void GetPostById_WhenPostFound_MapsAndReturns()
    {
        // Arrange
        const int postId = 1;

        var filteredPosts = _fixture.Build<Post>().With(x => x.Id, postId).CreateMany(1).AsQueryable();
        var postQueryable = _fixture.CreateMany<Post>(10).Concat(filteredPosts).AsQueryable();
        
        _database.Get<Post>().Returns(postQueryable);

        var postDtoQueryable = _fixture.Build<PostDto>().With(x => x.Id, postId).CreateMany(1).AsQueryable();
        
        _mapper.ProjectTo<PostDto>(Arg.Any<IQueryable<Post>>())
            .Returns(postDtoQueryable);
        
        var postDto = postDtoQueryable.SingleOrDefault();
        
        var service = GetService();
        
        // Act
        var result = service.GetPostById(postId);

        // Assert
        _database.Received(1).Get<Post>();
        
        _mapper.Received(1).ProjectTo<PostDto>(Arg.Is<IQueryable<Post>>(
            input => input.SequenceEqual(filteredPosts)));
        
        result.Should().BeEquivalentTo(postDto);
    }

    [Fact]
    public void GetPostById_WhenNoPostFound_ReturnsNullObject()
    {
        // Arrange
        const int postId = 1;
        var service = GetService();

        // Act
        var result = service.GetPostById(postId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetPostsAfterDate_WhenPostsFound_MappedAndReturned()
    {
        // Arrange
        var date = DateTime.UtcNow;
        
        var filteredPosts = _fixture.Customize(new IncrementingDateTimeCustomization())
            .Build<Post>()
            .CreateMany(10).AsQueryable();

        var postQueryable = _fixture.Build<Post>()
            .With(x => x.CreatedDate, date)
            .CreateMany(10).Concat(filteredPosts).AsQueryable();

        _database.Get<Post>().Returns(postQueryable);
        
        var postDtoQueryable = _fixture.Customize(new IncrementingDateTimeCustomization())
            .Build<PostDto>()
            .CreateMany(10).AsQueryable();

        _mapper.ProjectTo<PostDto>(Arg.Any<IQueryable<Post>>())
            .Returns(postDtoQueryable);
        
        var postsDto = postDtoQueryable.ToArray();

        var service = GetService();

        // Act
        var result = service.GetPostsAfterDate(date);

        // Assert
        _database.Received(1).Get<Post>();

        _mapper.Received(1).ProjectTo<PostDto>(Arg.Is<IQueryable<Post>>(
            input => input.SequenceEqual(filteredPosts)));
        
        result.Should().BeEquivalentTo(postsDto);
    }

    [Fact]
    public void GetPostsAfterDate_WhenNoPostsFound_ReturnsEmptyArray()
    {
        // Arrange
        var date = DateTime.UtcNow;
        var expectedResult = Array.Empty<PostDto>();
        var service = GetService();

        // Act
        var result = service.GetPostsAfterDate(date);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void GetAllResourceTypes_WhenResourcesFound_MapsAndReturns()
    {
        // Arrange
        var allResourceTypesQueryable = _fixture.Build<ResourceType>().CreateMany(10).AsQueryable();
        _database.Get<ResourceType>().Returns(allResourceTypesQueryable);
        
        var resourceTypesDtoQueryable = _fixture.Build<ResourceTypeDto>().CreateMany(10).AsQueryable();
        _mapper.ProjectTo<ResourceTypeDto>(Arg.Any<IQueryable<ResourceType>>()).Returns(resourceTypesDtoQueryable);

        var expectedResult = resourceTypesDtoQueryable.ToArray();
        
        var service = GetService();

        // Act
        var result = service.GetAllResourceTypes();

        // Assert
        _database.Received(1).Get<ResourceType>();

        _mapper.Received(1).ProjectTo<ResourceTypeDto>(Arg.Is<IQueryable<ResourceType>>(
            input => input.SequenceEqual(allResourceTypesQueryable)));

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void GetAllResourceTypes_WhenNoResourcesFound_ReturnsEmptyIQueryable()
    {
        // Arrange
        _database.Get<ResourceType>().Returns(Enumerable.Empty<ResourceType>().AsQueryable());

        var service = GetService();

        // Act
        var result = service.GetAllResourceTypes();

        // Assert
        _database.Received(1).Get<ResourceType>();

        _mapper.Received(1).ProjectTo<ResourceTypeDto>(Arg.Is<IQueryable<ResourceType>>(
            input => input.IsNullOrEmpty()));

        result.Should().BeEmpty();
    }
    
    [Fact]
    public void UpdatePost_IfPostExists_UpdatePostWithNewValues()
    {
        // Arrange
        const int postId = 1;
        var newDesc = _fixture.Create<string>();

        var existingDomainPost = _fixture.Build<Post>().With(x => x.Id, postId).CreateMany(1).AsQueryable();

        var updatePostDto = _fixture.Build<UpdatePostDto>().With(x => x.Id, postId).Create();

        _database.Get<Post>().Returns(existingDomainPost);

        _mapper.When(x => x.Map(updatePostDto, existingDomainPost)).Do(x =>
        {
            existingDomainPost.ElementAt(0).Description = newDesc;
        });

        var service = GetService();
        
        // Act
        service.UpdatePost(updatePostDto);
        
        // Assert
        _database.When(x => x.SaveChanges()).Do(x =>
        {
            existingDomainPost.ElementAt(0).Description.Should().Be(newDesc);
        });
    }

    [Fact]
    public void UpdatePost_IfPostDoesNotExist_ExceptionThrown()
    {
        // Arrange
        const int postId = 1;

        var updatePostDto = _fixture.Build<UpdatePostDto>().With(x => x.Id, postId).Create();

        var service = GetService();
        // Act
        var act = () =>  service.UpdatePost(updatePostDto);

        // Assert
        act.Should().Throw<NotFoundException>().WithMessage($"No post found for id: {postId}");
    }

    [Fact]
    public void CreatePost_WhenCorrectArgsPassed_CreateNewPost()
    {
        // Arrange
        const int postId = 1;
        var mockDb = new Dictionary<int, Post>();

        var createPostDto = _fixture.Build<CreatePostDto>().Create();
        var postDomain = _fixture.Build<Post>().With(x => x.Id, postId).Create();

        _mapper.Map<Post>(createPostDto).Returns(postDomain);
        
        _database.When(x => x.Add(postDomain)).Do(x =>
        {
            mockDb.Add(postDomain.Id, postDomain);
        });

        var service = GetService();

        // Act
        service.CreatePost(createPostDto);

        // Assert
        _mapper.Received(1).Map<Post>(Arg.Is(createPostDto));
        
        _database.When(x => x.SaveChanges()).Do(x =>
        {
            mockDb[postId].Should().BeEquivalentTo(postDomain);
        });
    }

    [Fact]
    public void DeletePost_WhenPostExists_DeletePost()
    {
        // Arrange
        const int postId = 1;
        
        var postQueryable = _fixture.Build<Post>()
            .With(x => x.Id, postId)
            .With(x => x.Active, true)
            .CreateMany(1)
            .AsQueryable();

        _database.Get<Post>().Returns(postQueryable);

        var service = GetService();

        // Act
        service.DeletePost(postId);

        // Assert
        _database.When(x => x.SaveChanges()).Do(x =>
        {
            postQueryable.ElementAt(0).Active.Should().Be(false);
        });
    }

    [Fact]
    public void DeletePost_IfPostDoesNotExist_ExceptionThrown()
    {
        // Arrange
        const int postId = 1;

        var service = GetService();
        
        // Act
        var act = () => service.DeletePost(postId);

        // Assert
        act.Should().Throw<NotFoundException>().WithMessage($"No post found for postId: {postId}");
    }

    [Fact]
    public void RetrievePostForId_WhenPostFound_ReturnsIQueryableOfPost()
    {
        // Arrange
        const int postId = 1;
        var filteredPostQueryable = _fixture.Build<Post>().With(x => x.Id, postId).CreateMany(1).AsQueryable();
        var allPostsQueryable = _fixture.Build<Post>().CreateMany(10).Concat(filteredPostQueryable).AsQueryable();

        _database.Get<Post>().Returns(allPostsQueryable);

        var service = GetService();
        
        // Act
        var result = service.RetrievePostForId(postId);

        // Assert
        _database.Received(1).Get<Post>();

        result.Should().BeEquivalentTo(filteredPostQueryable);
    }

    [Fact]
    public void RetrievePostForId_WhenNoPostFound_ReturnsEmptyIQueryable()
    {
        // Arrange
        const int postId = 1;

        _database.Get<Post>().Returns(Enumerable.Empty<Post>().AsQueryable());
        
        var service = GetService();
        
        // Act
        var result = service.RetrievePostForId(postId);

        // Assert
        _database.Received(1).Get<Post>();
        
        result.Should().BeEmpty();
    }

    [Fact]
    public void RetrievePostResourceForId_WhenResourceFound_ReturnsIQueryableOfResource()
    {
        // Arrange
        const int postResourceId = 1;
        var filteredPostResourceQueryable = _fixture.Build<PostResource>().With(x => x.Id, postResourceId).CreateMany(1).AsQueryable();
        var allPostResourcesQueryable = _fixture.Build<PostResource>().CreateMany(10).Concat(filteredPostResourceQueryable).AsQueryable();

        _database.Get<PostResource>().Returns(allPostResourcesQueryable);

        var service = GetService();
        
        // Act
        var result = service.RetrievePostResourceForId(postResourceId);

        // Assert
        _database.Received(1).Get<PostResource>();

        result.Should().BeEquivalentTo(filteredPostResourceQueryable);
    }

    [Fact]
    public void RetrievePostResourceForId_WhenNoResourceFound_ReturnsEmptyIQueryable()
    {
        // Arrange
        const int postResourceId = 1;

        _database.Get<PostResource>().Returns(Enumerable.Empty<PostResource>().AsQueryable());
        
        var service = GetService();
        
        // Act
        var result = service.RetrievePostResourceForId(postResourceId);

        // Assert
        _database.Received(1).Get<PostResource>();
        
        result.Should().BeEmpty();
    }

    [Fact]
    public void UpdatePostResource_IfPostResourceExists_PostResourceUpdated()
    {
        // Arrange
        const int postResourceId = 1;
        var updatedValue = _fixture.Create<string>();

        var existingDomainPostResource = _fixture.Build<PostResource>().With(x => x.Id, postResourceId).CreateMany(1).AsQueryable();

        var updatePostResourceDto = _fixture.Build<UpdatePostResourceDto>()
            .With(x => x.Id, postResourceId)
            .With(x => x.Value, updatedValue)
            .Create();

        _database.Get<PostResource>().Returns(existingDomainPostResource);
        
        _mapper.When(x => x.Map(updatePostResourceDto, existingDomainPostResource)).Do(x =>
        {
            existingDomainPostResource.ElementAt(0).Title = updatedValue;
        });

        var service = GetService();

        // Act
        service.UpdatePostResource(updatePostResourceDto);

        // Assert
        _database.When(x => x.SaveChanges()).Do(x =>
        {
            existingDomainPostResource.ElementAt(0).Title.Should().Be(updatedValue);
        });
    }

    [Fact]
    public void UpdatePostResource_IfNoPostResourceFound_ExceptionThrown()
    {
        // Arrange
        const int postResourceId = 1;

        var updatePostResourceDto = _fixture.Build<UpdatePostResourceDto>().With(x => x.Id, postResourceId).Create();

        var service = GetService();
        // Act
        var act = () =>  service.UpdatePostResource(updatePostResourceDto);

        // Assert
        act.Should().Throw<NotFoundException>().WithMessage($"Cannot find post resource for id: {postResourceId}");
    }
    
    private PostService GetService()
    {
        return new PostService(_database, _mapper);
    }
}