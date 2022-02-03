using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dbworker.Data.EF
{
    public partial class User
    {
        public int Id { get; set; }
        public Guid? Guid { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int? Org { get; set; }
        public string Email { get; set; }

        public virtual Org OrgNavigation { get; set; }

        public override string ToString()
        {
            return $"[{Guid}] User: {Name} {Surname} {Patronymic} ({Email})";
        }
    }
}