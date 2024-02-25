using Customer.Context;
using Customer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Customer.Services
{
    public interface ICustomerService
    {
        CustomerDetails GetCustomerById(int id);
        IEnumerable<CustomerDetails> GetAllCustomers();
        void CreateCustomer(CustomerDetails customer);
    }

    public class CustomerService : ICustomerService
    {
        private readonly CustomerDbContext _context;

        public CustomerService(CustomerDbContext context)
        {
            _context = context;
        }

        public CustomerDetails GetCustomerById(int id)
        {
            return _context.Customers.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<CustomerDetails> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public void CreateCustomer(CustomerDetails customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }
    }
}
