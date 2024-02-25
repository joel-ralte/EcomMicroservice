using Microsoft.EntityFrameworkCore;
using Customer.Models;

namespace Customer.Context
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<CustomerDetails> Customers { get; set; }
    }
}
