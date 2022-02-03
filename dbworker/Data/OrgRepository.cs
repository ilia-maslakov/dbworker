using dbworker.Data.EF;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace dbworker.Data
{
    public class OrgRepository : IRepository<Org>
    {
        private readonly ILogger<UnitOfWork> _logger;
        private readonly DBworkerContext _context;

        public OrgRepository(DBworkerContext context)
        {
            _context = context;
            _logger = LoggerFactory.Create(options => { }).CreateLogger<UnitOfWork>();
        }

        Org IRepository<Org>.Get(int id)
        {
            return _context.Org.Find(id);
        }

        IEnumerable<Org> IRepository<Org>.Get()
        {
            return _context.Org.ToList();
        }

        IEnumerable<Org> IRepository<Org>.Get(Func<Org, bool> predicate)
        {
            return _context.Org.Where(predicate).ToList();
        }

        public ValueTask<Org> GetAsync(int id)
        {
            return _context.Org.FindAsync(id);
        }

        public Task<List<Org>> GetAsync()
        {
            return _context.Org.ToListAsync();
        }


        Org IRepository<Org>.Add(Org item)
        {
            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Try Add org ({item})");
            _context.Add(item);
            return item;
        }

        ValueTask<EntityEntry<Org>> IRepository<Org>.AddAsync(Org item)
        {
            return _context.AddAsync(item);
        }

        void IRepository<Org>.Update(Org item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        void IRepository<Org>.Remove(int id)
        {
            Org o = _context.Org.Find(id);
            if (o != null)
            {
                _context.Org.Remove(o);
            }
        }

        void IRepository<Org>.Remove(Org item)
        {
            _context.Org.Remove(item);
        }

        void IRepository<Org>.Remove(Func<Org, bool> predicate)
        {
            foreach (Org o in _context.Org.Where(predicate).ToList())
            {
                _context.Org.Remove(o);
            }
        }
    }
}