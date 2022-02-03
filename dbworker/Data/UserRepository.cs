using dbworker.Data.EF;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace dbworker.Data
{
    public class UserRepository : IRepository<User>
    {
        private readonly ILogger<UnitOfWork> _logger;
        private readonly DBworkerContext _context;

        public UserRepository(DBworkerContext context)
        {
            _context = context;
            _logger = LoggerFactory.Create(options => { }).CreateLogger<UnitOfWork>();
        }


        User IRepository<User>.Get(int id)
        {
            return _context.User.Find(id);
        }

        IEnumerable<User> IRepository<User>.Get()
        {
            return _context.User.ToList();
        }

        IEnumerable<User> IRepository<User>.Get(Func<User, bool> predicate)
        {
            return _context.User.Where(predicate).ToList();
        }

        public IEnumerable<User> Get(int page = 1, int pageSize = 20)
        {
            try
            {
                return _context.User.OrderBy(i => i.Id).Skip(pageSize * page).Take(pageSize).ToList();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Error: {e.Message}");
                return null;
            }
        }

        public ValueTask<User> GetAsync(int id)
        {
            return _context.User.FindAsync(id);
        }

        Task<List<User>> IRepository<User>.GetAsync()
        {
            return _context.User.ToListAsync();
        }

        User IRepository<User>.Add(User item)
        {
            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Try Add user({item})");
            _context.Add(item);
            return item;
        }

        ValueTask<EntityEntry<User>> IRepository<User>.AddAsync(User item)
        {
            return _context.AddAsync(item);
        }

        void IRepository<User>.Update(User item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        void IRepository<User>.Remove(int id)
        {
            User u = _context.User.Find(id);
            if (u != null)
            {
                _context.User.Remove(u);
            }
        }

        void IRepository<User>.Remove(User item)
        {
            _context.User.Remove(item);
        }

        void IRepository<User>.Remove(Func<User, bool> predicate)
        {
            foreach (User o in _context.User.Where(predicate).ToList())
            {
                _context.User.Remove(o);
            }
        }

    }
}

/*
        public IList<User> GetUsers()
        {
            int orgid = 0; //OrgId ?? 0;

            IQueryable<User> l = _context.User;

            if (orgid > 0)
            {
                l = l.Where(p => p.Org == orgid);
            }
            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} UserList retern {l.Count()} records");

            return l.ToList<User>();
        }

        public User Add(User user)
        {
            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Try Add user({user.Name}, {user.Surname}, {user.Patronymic})");

            _context.Add(user);
            try
            {
                _context.SaveChanges();
                _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Add success Id = {user.Id}");

            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Error: {e.Message}");
            }

            return user;
        }

        public bool Delete(int id)
        {
            var u = _context.User.Find(id);
            if (u != null)
            {
                _context.User.Remove(u);
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Error Delete[{ e.Message}]");
                }
            }
            return false;
        }

        public User Find(int id)
        {
            return new User { Id = -1, Name = "test" };
        }

        public bool OrgExists(int orgid)
        {
            var u = _context.Org.Find(orgid);
            if (u != null)
            {
                return true;
            }
            return false;
        }

        public bool UserExists(int id)
        {
            var u = _context.User.Find(id);
            if (u != null)
            {
                return true;
            }
            return false;
        }

        public string LinkUserOrg(int id, int orgid)
        {
            var u = _context.User.Find(id);

            if (u == null)
            {
                return $"user id = ({id}) not found";
            }
            if (!OrgExists(orgid))
            {
                return $"org id = ({orgid}) not found";
            }

            u.Org = orgid;
            _context.Entry(u).State = EntityState.Modified;

            try
            {
                 _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                return $"Error LinkUserOrg [{e.Message}]";
            }
            return "OK";
        }

public virtual void Dispose(bool disposing)
{
    if (needispose) { 
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }
}

public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
*/
