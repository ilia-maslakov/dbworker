using dbworker.Data.EF;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dbworker.Connection
{
    public interface IUserRepository<T> : IDisposable
        where T : class
    {
        public void Reconect();
        Task<User> Add(User user);
        bool Delete(int id);
        User Find(int id);
        IList<User> GetUsers(int page, int maxRecords);
        bool OrgExists(int orgid);
        bool UserExists(int id);
        string LinkUserOrg(int id, int orgid);
    }
}