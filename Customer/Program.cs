using Microsoft.EntityFrameworkCore;
using Customer.Context;
using Customer.Services;

namespace Customer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<CustomerDbContext>(options =>
            {
                options.UseSqlite("Data Source=customers.db");
            });

            builder.Services.AddHttpClient();

            builder.Services.AddScoped<ICustomerService, CustomerService>();

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
