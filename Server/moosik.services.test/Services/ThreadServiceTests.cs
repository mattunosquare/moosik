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
using moosik.services.Exceptions;
using moosik.services.Services;
using NSubstitute;
using Xunit;

namespace moosik.services.test.Services;

public class ThreadServiceTests
{
    private readonly IMoosikDatabase _database;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public ThreadServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        _database = Substitute.For<IMoosikDatabase>();
        _mapper = Substitute.For<IMapper>();
    }

    [Fact]
    public void GetAllThreads_WhenCalledWithNoArgs_MapsAndReturnsAll()
    {
        // Arrange
        var threads = _fixture.Build<Thread>().CreateMany().AsQueryable();
        _database.Get<Thread>().Returns(threads);

        var threadsDto = _fixture.Build<ThreadDto>().CreateMany().AsQueryable();
        _mapper.ProjectTo<ThreadDto>(Arg.Any<IQueryable<Thread>>()).Returns(threadsDto);

        var expectedResult = threadsDto.ToArray();

        var service = GetService();

        // Act
        var result = service.GetAllThreads();

        // Assert
        _database.Received(1).Get<Thread>();

        _mapper.Received(1).ProjectTo<ThreadDto>(Arg.Is<IQueryable<Thread>>(
            input => input.SequenceEqual(threads)));

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void GetAllThreads_WhenCalledWithUserId_MapsAndReturnsThreadsMatchingUserId()
    {
        // Arrange
        const int userId = 1;
        var filteredThreadsQueryable = _fixture.Build<Thread>().With(x => x.UserId, userId).CreateMany(1).AsQueryable();
        var allThreadsQueryable = _fixture.Build<Thread>().CreateMany(10).Concat(filteredThreadsQueryable).AsQueryable();

        _database.Get<Thread>().Returns(allThreadsQueryable);

        var threadDtoQueryable = _fixture.Build<ThreadDto>().With(x => x.UserId, userId).CreateMany(1).AsQueryable();

        _mapper.ProjectTo<ThreadDto>(Arg.Any<IQueryable<Thread>>()).Returns(threadDtoQueryable);

        var expectedResult = threadDtoQueryable.ToArray();

        var service = GetService();

        // Act
        var result = service.GetAllThreads(userId);

        // Assert
        _database.Received(1).Get<Thread>();

        _mapper.Received(1).ProjectTo<ThreadDto>(Arg.Is<IQueryable<Thread>>(
            input => input.SequenceEqual(filteredThreadsQueryable)));

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void GetAllThreads_WhenNoThreadsFound_ReturnsEmptyArray()
    {
        // Arrange
        var expectedResult = Array.Empty<ThreadDto>();
        var service = GetService();

        // Act
        var result = service.GetAllThreads();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void GetThreadById_WhenThreadFound_ReturnsThread()
    {
        // Arrange
        const int threadId = 1;
        var filteredThreadsQueryable = _fixture.Build<Thread>().With(x => x.Id, threadId).CreateMany(1).AsQueryable();
        var allThreadsQueryable = _fixture.Build<Thread>().CreateMany(10).Concat(filteredThreadsQueryable).AsQueryable();
        _database.Get<Thread>().Returns(allThreadsQueryable);
        
        var threadDtoQueryable =
            _fixture.Build<ThreadDto>().With(x => x.Id, threadId).CreateMany(1).AsQueryable();

        _mapper.ProjectTo<ThreadDto>(Arg.Any<IQueryable<Thread>>()).Returns(threadDtoQueryable);

        var expectedResult = threadDtoQueryable.SingleOrDefault();

        var service = GetService();

        // Act
        var result = service.GetThreadById(threadId);

        // Assert
        _database.Received(1).Get<Thread>();

        _mapper.Received(1).ProjectTo<ThreadDto>(Arg.Is<IQueryable<Thread>>(
            input => input.SequenceEqual(filteredThreadsQueryable)));

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void GetThreadBy_WhenNoThreadFound_ReturnsNullObject()
    {
        // Arrange
        const int threadId = 1;

        _database.Get<Thread>().Returns(Enumerable.Empty<Thread>().AsQueryable());

        var service = GetService();
        
        // Act
        var result = service.GetThreadById(threadId);

        // Assert
        _database.Received(1).Get<Thread>();

        _mapper.Received(1).ProjectTo<ThreadDto>(Arg.Is<IQueryable<Thread>>(
            input => input.IsNullOrEmpty()));
        
        result.Should().BeNull();
    }

    [Fact]
    public void GetThreadsAfterDate_WhenThreadsFound_MapsAndReturns()
    {
        // Arrange
        var date = DateTime.UtcNow;
        
        var filteredThreadsQueryable = _fixture.Customize(new IncrementingDateTimeCustomization())
            .Build<Thread>()
            .CreateMany(10).AsQueryable();

        var allThreadsQueryable = _fixture.Build<Thread>()
            .With(x => x.CreatedDate, date)
            .CreateMany(10).Concat(filteredThreadsQueryable).AsQueryable();

        _database.Get<Thread>().Returns(allThreadsQueryable);
        
        var threadDtoQueryable = _fixture.Customize(new IncrementingDateTimeCustomization())
            .Build<ThreadDto>()
            .CreateMany(10).AsQueryable();

        _mapper.ProjectTo<ThreadDto>(Arg.Any<IQueryable<Thread>>())
            .Returns(threadDtoQueryable);
        
        var expectedResult = threadDtoQueryable.ToArray();

        var service = GetService();
        
        // Act
        var result = service.GetThreadsAfterDate(date);

        // Assert
        _database.Received(1).Get<Thread>();

        _mapper.Received(1).ProjectTo<ThreadDto>(Arg.Is<IQueryable<Thread>>(
            input => input.SequenceEqual(filteredThreadsQueryable)));

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void GetThreadAfterDate_WhenNoThreadFound_ReturnsEmptyArray()
    {
        // Arrange
        var date = DateTime.UtcNow;
        var expectedResult = Array.Empty<ThreadDto>();
        var service = GetService();

        // Act
        var result = service.GetThreadsAfterDate(date);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void RetrieveThreadForId_WhenThreadFound_ReturnsIQueryableOfThread()
    {
        // Arrange
        const int threadId = 1;
        var filteredThreadsQueryable = _fixture.Build<Thread>().With(x => x.Id, threadId).CreateMany(1).AsQueryable();
        var allThreadsQueryable = _fixture.Build<Thread>().CreateMany(10).Concat(filteredThreadsQueryable).AsQueryable();

        _database.Get<Thread>().Returns(allThreadsQueryable);

        var service = GetService();
        
        // Act
        var result = service.RetrieveThreadForId(threadId);

        // Assert
        _database.Received(1).Get<Thread>();

        result.Should().BeEquivalentTo(filteredThreadsQueryable);
    }
    
    [Fact]
    public void UpdateThread_IfThreadExists_UpdateThreadWithNewValues()
    {
        // Arrange
        const int threadId = 1;
        var updatedTitle = _fixture.Create<string>();

        var existingDomainThread = _fixture.Build<Thread>().With(x => x.Id, threadId).CreateMany(1).AsQueryable();

        var updateThreadDto = _fixture.Build<UpdateThreadDto>()
            .With(x => x.Id, threadId)
            .With(x => x.Title, updatedTitle)
            .Create();

        _database.Get<Thread>().Returns(existingDomainThread);
        
        _mapper.When(x => x.Map(updateThreadDto, existingDomainThread)).Do(x =>
        {
            existingDomainThread.ElementAt(0).Title = updatedTitle;
        });

        var service = GetService();

        // Act
        service.UpdateThread(updateThreadDto);

        // Assert
        _database.When(x => x.SaveChanges()).Do(x =>
        {
            existingDomainThread.ElementAt(0).Title.Should().Be(updatedTitle);
        });
    }

    [Fact]
    public void UpdateThread_IfThreadDoesNotExist_ExceptionThrown()
    {
        // Arrange
        const int threadId = 1;

        var updatePostDto = _fixture.Build<UpdateThreadDto>().With(x => x.Id, threadId).Create();

        var service = GetService();

        // Act
        var act = () => service.UpdateThread(updatePostDto);

        // Assert
        act.Should().Throw<NotFoundException>().WithMessage($"No thread found for thread with id: {threadId}");
    }

    [Fact]
    public void CreateThread_WhenCorrectArgsPassed_CreateNewThread()
    {
        // Arrange
        const int threadId = 1;
        var mockDb = new Dictionary<int, Thread>();

        var createThreadDto = _fixture.Build<CreateThreadDto>().Create();
        var threadDomain = _fixture.Build<Thread>().With(x =>x.Id, threadId).Create();

        _mapper.Map<Thread>(createThreadDto).Returns(threadDomain);
        
        _database.When(x => x.Add(threadDomain)).Do(x =>
        {
            mockDb.Add(threadDomain.Id, threadDomain);
        });
        
        var service = GetService();
        
        // Act
        service.CreateThread(createThreadDto);
        
        // Assert
        _mapper.Received(1).Map<Thread>(Arg.Is(createThreadDto));
        
        _database.When(x => x.SaveChanges()).Do(x =>
        {
            mockDb[threadId].Should().BeEquivalentTo(threadDomain);
        });

    }

    [Fact]
    public void DeleteThread_WhenThreadExists_DeleteThread()
    {
        // Arrange
        const int threadId = 1;
        
        var threadQueryable = _fixture.Build<Thread>()
            .With(x => x.Id, threadId)
            .With(x => x.Active, true)
            .CreateMany(1)
            .AsQueryable();

        _database.Get<Thread>().Returns(threadQueryable);

        var service = GetService();

        // Act
        service.DeleteThread(threadId);

        // Assert
        _database.When(x => x.SaveChanges()).Do(x =>
        {
            threadQueryable.ElementAt(0).Active.Should().Be(false);
        });
    }

    [Fact]
    public void DeleteThread_IfThreadDoesNotExist_ExceptionThrown()
    {
        // Arrange
        const int threadId = 1;

        var service = GetService();
        
        // Act
        var act = () => service.DeleteThread(threadId);

        // Assert
        act.Should().Throw<NotFoundException>().WithMessage($"No thread found for threadId: {threadId}");
    }

    [Fact]
    public void RetrieveThreadForId_WhenNoThreadFound_ReturnsEmptyIQueryable()
    {
        // Arrange
        const int threadId = 1;
        
        _database.Get<Thread>().Returns(Enumerable.Empty<Thread>().AsQueryable());

        var service = GetService();

        // Act
        var result = service.RetrieveThreadForId(threadId);

        // Assert
        _database.Received(1).Get<Thread>();
        
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetAllThreadTypes_WhenThreadTypesFound_MapsAndReturns()
    {
        // Arrange
        var allThreadTypesQueryable = _fixture.Build<ThreadType>().CreateMany(10).AsQueryable();
        _database.Get<ThreadType>().Returns(allThreadTypesQueryable);
        
        var threadTypesDtoQueryable = _fixture.Build<ThreadTypeDto>().CreateMany(10).AsQueryable();
        _mapper.ProjectTo<ThreadTypeDto>(Arg.Any<IQueryable<ThreadType>>()).Returns(threadTypesDtoQueryable);

        var expectedResult = threadTypesDtoQueryable.ToArray();
        
        var service = GetService();

        // Act
        var result = service.GetAllThreadTypes();

        // Assert
        _database.Received(1).Get<ThreadType>();

        _mapper.Received(1).ProjectTo<ThreadTypeDto>(Arg.Is<IQueryable<ThreadType>>(
            input => input.SequenceEqual(allThreadTypesQueryable)));

        result.Should().BeEquivalentTo(expectedResult);

    }

    [Fact]
    public void GetAllThreadTypes_WhenNoThreadTypesFound_ReturnsEmptyIQueryable()
    {
        // Arrange
        _database.Get<ThreadType>().Returns(Enumerable.Empty<ThreadType>().AsQueryable());

        var service = GetService();

        // Act
        var result = service.GetAllThreadTypes();

        // Assert
        _database.Received(1).Get<ThreadType>();

        _mapper.Received(1).ProjectTo<ThreadTypeDto>(Arg.Is<IQueryable<ThreadType>>(
            input => input.IsNullOrEmpty()));

        result.Should().BeEmpty();
    }
    
    private ThreadService GetService()
    {
        return new ThreadService(_database, _mapper);
    }
}