using Microsoft.EntityFrameworkCore;

namespace Wedding_Planner.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) {}

        // Add Db sets here.
        public DbSet<User> Users {get;set;}
        public DbSet<Wedding> Weddings {get;set;}
        public DbSet<Guest> Guests {get;set;}

    }
}