using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Contracts
{
    public interface ICartItemManager
    {
        Task<IEnumerable<CartItem>> GetAllAsync();
        Task<CartItem> GetByIdAsync(int id);
        Task AddAsync(CartItem item);
        Task UpdateAsync(CartItem item);
        Task DeleteAsync(int id);
        Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId);
        Task AddOrUpdateCartItemAsync(string userId, int productId, int quantity);


} }
