using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using moosik.dal.Contexts;
using moosik.services.Dtos;
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
    
    public ICollection<ThreadDto> GetAllThreads(int? userId)
    {
        var threads = _database.Threads
            .Include(t => t.Posts)
            .Include(t => t.User)
            .Include(t => t.ThreadType)
            .AsQueryable();

        //Filter down if a userId has been provided

        if (userId != null)
        {
            threads = threads.Where(t => t.Id == userId);
        }

        var threadDtos = _mapper.Map<ICollection<ThreadDto>>(threads);
        return threadDtos;
    }

    public ThreadDto GetThreadById(int threadId)
    {
        throw new NotImplementedException();
    }

    public ICollection<ThreadDto> GetThreadsAfterDate(DateTime date)
    {
        throw new NotImplementedException();
    }

    public void UpdateThread(ThreadDto thread)
    {
        throw new NotImplementedException();
    }

    public void CreateThread(ThreadDto thread)
    {
        throw new NotImplementedException();
    }

    public void DeleteThreadById(int threadId)
    {
        throw new NotImplementedException();
    }

    public ICollection<ThreadTypeDto> GetAllThreadTypes()
    {
        var threadTypes = _database.ThreadTypes.AsQueryable();

        var threadTypesDto = _mapper.Map<ICollection<ThreadTypeDto>>(threadTypes).ToList();
        return threadTypesDto;
    }
}