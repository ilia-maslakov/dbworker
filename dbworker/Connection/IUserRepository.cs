using dbworker.Data.EF;
using System.Collections.Generic;

namespace dbworker.Connection
{
    public interface IRepository<T>
    {
        IEnumerable<T> All(int page, int maxRecords);
        T Create(T user);
        void Delete(int id);
        T Find(int id);
        T Single(int id);
        T Update(int id, T model);
    }

    public interface IUserRepository : IRepository<User> { }
}