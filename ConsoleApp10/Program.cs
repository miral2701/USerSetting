using Microsoft.EntityFrameworkCore;

namespace ConsoleApp10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext()) { 
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                List<User> users = new List<User>()
                {
                    new User{Email="mira@gmail.com",UserSettings=new UserSettings{Country="Ukraine",City="Odesa"}},
                    new User{Email="mira2701@stud.onu.edu.ua",UserSettings=new UserSettings{Country="Ukraine",City="Odesa"}},
                    new User{Email="sigma@gmail.com",UserSettings=new UserSettings{Country="Ukraine",City="Odesa"}},

                };
                db.Users.AddRange(users);
                db.SaveChanges();
            }

            using (ApplicationContext db = new ApplicationContext()) {
                var currentUser = db.Users.Include(e => e.UserSettings).FirstOrDefault();
                var user2 = db.Users.FirstOrDefault(e => e.Id == 2);

                var user3 = db.Users.FirstOrDefault(u=> u.Id == 3);
                if (user3 != null)
                {
                    db.Users.Remove(user3);
                    db.SaveChanges();
                }
            }
        }
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserSettings> UsersSettings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=testdb;Trusted_Connection=True;");
          
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasOne(e => e.UserSettings).WithOne(e => e.User);
            base.OnModelCreating(modelBuilder);
        }
    }

    public class UserSettings
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public int UserId{ get; set; }
        public User User { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public UserSettings UserSettings { get; set; }
    }
}
