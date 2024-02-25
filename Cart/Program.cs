using Microsoft.EntityFrameworkCore;
using Cart.Context;
using Cart.Services;

namespace Cart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<CartDbContext>(options =>
            {
                options.UseSqlite("Data Source=carts.db");
            });
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<ICartService, CartService>();

            builder.Services.AddControllers();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
