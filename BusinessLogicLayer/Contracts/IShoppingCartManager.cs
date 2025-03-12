using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Contracts
{
    public interface IShoppingCartManager
    {
        Task<IEnumerable<ShoppingCart>> GetAllCartsAsync();
        Task<ShoppingCart> GetCartByIdAsync(int id);
        Task<ShoppingCart> GetCartByUserIdAsync(string userId);
        Task AddCartAsync(ShoppingCart cart);
        Task UpdateCartAsync(ShoppingCart cart);
        Task DeleteCartAsync(int id);
        Task AddItemToCartAsync(string userId, int productId, int quantity);
        Task RemoveItemFromCartAsync(string userId, int productId);
        Task ClearCartAsync(string userId);

    }
}
