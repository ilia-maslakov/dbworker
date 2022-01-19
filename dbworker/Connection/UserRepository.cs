using dbworker.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using dbworker.Controllers;

namespace dbworker.Connection
{
    public class UserRepository : IUserRepository
    {
        public IEnumerable<User> All(int page, int maxRecords)
        {
            // Доделать пагинацию
            return new List<User>();
        }


        public User Create(User user)
        {
            return user;
        }

        public User Single(int id)
        {
            return new User { Id = -1, Name = "test" };
        }

        public User Update(int id, User user)
        {
            return user;
        }

        void IRepository<User>.Delete(int id)
        {
            throw new NotImplementedException();
        }

        public User Find(int id)
        {
            return new User { Id = -1, Name = "test" };
        }
    }
}