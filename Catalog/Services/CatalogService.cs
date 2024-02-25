using Catalog.Context;
using Catalog.Models;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Services
{
    public interface ICatalogService
    {
        Product GetProductByProdId(int id);
        IEnumerable<Product> GetAllProducts();
        void AddProduct(Product product);
    }

    public class ProductRepository : ICatalogService
    {
        private readonly CatalogDbContext _context;

        public ProductRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public Product GetProductByProdId(int Id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == Id);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
    }
}
