using DemoApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customer { get; set; }
    }
}
