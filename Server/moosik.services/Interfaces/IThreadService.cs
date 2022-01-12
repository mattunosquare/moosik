using System;
using System.Collections.Generic;
using moosik.services.Dtos;

namespace moosik.services.Interfaces;

public interface IThreadService
{
    ICollection<ThreadDto> GetAllThreads(int? userId);
    ThreadDto GetThreadById(int threadId);
    ICollection<ThreadDto> GetThreadsAfterDate(DateTime date);
    void UpdateThread(ThreadDto thread);
    void CreateThread(ThreadDto thread);
    void DeleteThreadById(int threadId);
    ICollection<ThreadTypeDto> GetAllThreadTypes();
}