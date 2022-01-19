using System.Collections.Generic;

namespace dbworker.Data.EF
{
    public partial class Org
    {
        public Org()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}