using dbworker.Data.EF;
using System.Threading.Tasks;

namespace dbworker.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBworkerContext _db;
        private UserRepository _userRepository;
        private OrgRepository _orgRepository;

        public UnitOfWork(DBworkerContext context)
        {
            _db = context;
        }

        public IRepository<User> Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_db);
                return _userRepository;
            }
        }

        public IRepository<Org> Orgs
        {
            get
            {
                if (_orgRepository == null)
                    _orgRepository = new OrgRepository(_db);
                return _orgRepository;
            }
        }

        int IUnitOfWork.SaveChanges()
        {
            return _db.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}
