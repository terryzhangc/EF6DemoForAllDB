using Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSqlite
{
    public class MyDbContext : DbContext
    {

        public MyDbContext()
            : base("name=DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new MyDbContext())
            {

                var destUser = context.Users.FirstOrDefault(x => x.Name == "User 1");

                if (destUser == null)
                {
                    destUser = context.Users.Add(new User
                    {
                        Name = "User 1",
                        Description = "test",
                        CreateOn = DateTime.UtcNow,
                        UpdateOn = DateTime.UtcNow
                    });
                    var count = context.SaveChanges();
                }

                destUser.Description = "Modified " + DateTime.UtcNow.ToString();
                context.Entry<User>(destUser).State = EntityState.Modified;
                var modifyCount = context.SaveChanges();


                //context.Users.Remove(destUser);
                //var deleteCount = context.SaveChanges();
            }
        }
    }
}
