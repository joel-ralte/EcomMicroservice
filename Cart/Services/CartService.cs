using Cart.Context;
using Cart.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cart.Services
{
    public interface ICartService
    {
        Task<IEnumerable<CartDetails>> GetAllCartItems();
        Task<IEnumerable<CartDetails>> GetCartDetailsByOrderId(int orderId);
        Task AddToCart(CartDetails cartDetails);
    }

    public class CartService : ICartService
    {
        private readonly CartDbContext _cartDbContext;

        public CartService(CartDbContext cartDbContext)
        {
            _cartDbContext = cartDbContext;
        }
        public async Task<IEnumerable<CartDetails>> GetAllCartItems()
        {
            return await _cartDbContext.CartDetailsList.ToListAsync();
        }
        public async Task<IEnumerable<CartDetails>> GetCartDetailsByOrderId(int orderId)
        {
            return await _cartDbContext.CartDetailsList.Where(cd => cd.OrderId == orderId).ToListAsync();
        }
        public async Task AddToCart(CartDetails cartDetails)
        {
            _cartDbContext.CartDetailsList.Add(cartDetails);
            await _cartDbContext.SaveChangesAsync();
        }
    }
}
