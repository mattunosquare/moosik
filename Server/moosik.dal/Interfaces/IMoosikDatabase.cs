using System.Linq;

namespace moosik.dal.Interfaces;

public interface IMoosikDatabase
{
    IQueryable<T> Get<T>() where T : class;
    T Add<T>(T item) where T : class;
    void Add<T>(params T[] items) where T : class;
    void AddAsync<T>(params T[] items) where T : class;
    void Delete<T>(params T[] items) where T : class;
    int SaveChanges();
}
   