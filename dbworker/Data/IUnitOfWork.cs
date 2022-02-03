using dbworker.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dbworker.Data
{
    public interface IUnitOfWork
    {
        IRepository<Org> Orgs { get; }
        IRepository<User> Users { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
