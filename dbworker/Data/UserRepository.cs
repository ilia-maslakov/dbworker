using dbworker.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using dbworker.Controllers;
using Microsoft.Extensions.Logging;
using dbworker.Validators;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace dbworker.Connection
{
    public class UserRepository : IUserRepository<User>
    {
        private readonly ILogger<UserController> _logger;

        private readonly DBworkerContext _context;
        private readonly bool needispose = false;
        private bool disposed = false;


        public UserRepository(DBworkerContext context)
        {
            _context = context;
            needispose = false;
        }
        /*
        public UserRepository(ILogger<UserController> logger, DBworkerContext context, int rule)
        {
            _logger = logger;
            _context = context;
            needispose = false;
            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} post rule ({rule})");
        }

        [ActivatorUtilitiesConstructor]
        public UserRepository(ILogger<UserController> logger, DBworkerContext context)
        {
            _logger = logger;
            _context = context;
            needispose = false;
            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} post rule ({0})");
            
            //  _logger = logger;
            //  _validator = new UserValidator();
            //  _context = new DBworkerContext();
            //  needispose = true;
            
        }
        */
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