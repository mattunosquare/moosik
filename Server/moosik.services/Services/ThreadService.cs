using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using moosik.dal.Contexts;
using moosik.dal.Models;
using moosik.services.Dtos;
using moosik.services.Exceptions;
using moosik.services.Interfaces;
namespace moosik.services.Services;

public class ThreadService : IThreadService
{
    private readonly MoosikContext _database;
    private readonly IMapper _mapper;

    public ThreadService(MoosikContext database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }
    
    public ThreadDto[] GetAllThreads(int? userId)
    {
        Expression<Func<Thread, bool>> returnAll = t => true;
        Expression<Func<Thread, bool>> returnSingle = t => t.Id == userId;
        var filterThreadByUserId = userId >= 0 ? returnSingle : returnAll; 

        return _mapper.ProjectTo<ThreadDto>(
                _database.Get<Thread>()
                    .Where(filterThreadByUserId))
            .ToArray();
    }

    public ThreadDto GetThreadById(int threadId)
    {
        return _mapper.ProjectTo<ThreadDto>(
            _database.Get<Thread>()
                .Where(thread => thread.Id == threadId))
            .SingleOrDefault();
    }

    public ThreadDto[] GetThreadsAfterDate(DateTime date)
    {
        return _mapper.ProjectTo<ThreadDto>(
                _database.Get<Thread>()
                    .Where(thread => thread.CreatedDate > date))
            .ToArray();
    }

    public void UpdateThread(UpdateThreadDto updateThreadDto)
    {
        var existingThread = RetrieveThreadForId(updateThreadDto.Id).SingleOrDefault();

        if (existingThread == null)
        {
            throw new NotFoundException($"No thread found for thread with id: {updateThreadDto.Id}");
        }

        _mapper.Map(updateThreadDto, existingThread);
        _database.SaveChanges();
    }

    public void CreateThread(CreateThreadDto createThread)
    {
        var thread = _mapper.Map<Thread>(createThread);
        _database.Add(thread);
        _database.SaveChanges();
    }

    public void DeleteThread(int threadId)
    {
        var thread = RetrieveThreadForId(threadId).SingleOrDefault();

        if (thread == null)
        {
            throw new NotFoundException($"No thread found for threadId: {threadId}");
        }

        thread.Active = false;
        _database.SaveChanges();
    }

    public IQueryable<Thread> RetrieveThreadForId(int threadId)
    {
        return _database
            .Get<Thread>()
            .Where(t => t.Id == threadId);
    }

    public ThreadTypeDto[] GetAllThreadTypes()
    {
        return _mapper.ProjectTo<ThreadTypeDto>(
                _database.Get<ThreadType>())
            .ToArray();
    }
}