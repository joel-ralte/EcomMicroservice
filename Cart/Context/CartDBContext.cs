using Microsoft.EntityFrameworkCore;
using Cart.Models;

namespace Cart.Context
{
    public class CartDbContext : DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<CartDetails> CartDetailsList { get; set; }
    }
}
