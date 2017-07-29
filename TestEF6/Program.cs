using Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEF6
{
    public abstract partial class BaseEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        private static bool IsTransient(BaseEntity obj)
        {
            return obj != null && Equals(obj.Id, default(int));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(int)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }
    }

    public partial class Customer : BaseEntity
    {
        private ICollection<CustomerRole> _customerRoles;


        public string Username { get; set; }
        public string Email { get; set; }

        public virtual ICollection<CustomerRole> CustomerRoles
        {
            get { return _customerRoles ?? (_customerRoles = new List<CustomerRole>()); }
            protected set { _customerRoles = value; }
        }

    }

    public partial class CustomerRole : BaseEntity
    {
        public string Name { get; set; }
    }




    public class MyDbContext : DbContext
    {
        public MyDbContext()
            : base("name=DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<CustomerRole> CustomerRole { get; set; }

        public DbSet<Customer> Customer { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<Customer>()
                .ToTable("Customer")
                .HasKey(x => x.Id)
                .HasMany(c => c.CustomerRoles)
                .WithMany()
                .Map(m => m.ToTable("Customer_CustomerRole_Mapping"));
            modelBuilder.Entity<CustomerRole>().ToTable("CustomerRole").HasKey(x => x.Id);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            using (var context = new MyDbContext())
            {
                context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

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


                context.Users.Remove(destUser);
                var deleteCount = context.SaveChanges();

                var str = "";

                var customers = context.Customer.Include(x => x.CustomerRoles).ToList();


          


            }
        }
    }
}
