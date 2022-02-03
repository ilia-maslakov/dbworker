using dbworker.Data.EF;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dbworker.Data
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        IEnumerable<T> Get();
        IEnumerable<T> Get(Func<T, Boolean> predicate);
        IEnumerable<T> Get(int page = 1, int pageSize = 20);
        ValueTask<T> GetAsync(int id);
        Task<List<T>> GetAsync();
        T Add(T item);
        ValueTask<EntityEntry<T>> AddAsync(T item);
        void Remove(int id);
        void Remove(T item);
        void Remove(Func<T, Boolean> predicate);
        void Update(T item);
    }
}