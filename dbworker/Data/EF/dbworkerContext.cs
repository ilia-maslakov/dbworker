using dbworker.Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dbworker.Data.EF
{
    public partial class DBworkerContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Org> Org { get; set; }

        public DBworkerContext()
        {

        }
        public DBworkerContext(DbContextOptions<DBworkerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Петр", Surname = "Петров" },
                new User { Id = 2, Name = "Иван", Surname = "Иванов" },
                new User { Id = 3, Name = "Семен", Surname = "Семенов" },
                new User { Id = 4, Name = "Николай", Surname = "Николаев" }
            );
            /*
            modelBuilder.HasAnnotation("dbusers", "0.01");

            modelBuilder.Entity<Org>(entity =>
            {
                entity.ToTable("Org");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasOne(d => d.OrgNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Org)
                    .HasConstraintName("FK_User_Org");
            });
            */
            //OnModelCreatingPartial(modelBuilder);

        }
        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
