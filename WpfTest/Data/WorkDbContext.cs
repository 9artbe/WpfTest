using Microsoft.EntityFrameworkCore;
using WpfTest.Models;

namespace WpfTest.Data
{
    public class WorkDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public WorkDbContext()
        {
            DbInitializer.Initialize(this);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=WorkDb;Trusted_Connection=True;")
                .EnableSensitiveDataLogging(true);
        }
    }
}