using System;
using System.Linq;
using moosik.dal.Models;
using moosik.services.Dtos;
using moosik.services.Dtos.Thread;

namespace moosik.services.Interfaces;

public interface IThreadService
{
    ThreadDto[] GetAllThreads(int? userId = null);
    ThreadDto GetThreadById(int threadId);
    ThreadDto[] GetThreadsAfterDate(DateTime date);
    void UpdateThread(UpdateThreadDto updateThreadDto);
    void CreateThread(CreateThreadDto thread);
    void DeleteThread(int threadId);
    IQueryable<Thread> RetrieveThreadForId(int threadId);
    ThreadTypeDto[] GetAllThreadTypes();
}