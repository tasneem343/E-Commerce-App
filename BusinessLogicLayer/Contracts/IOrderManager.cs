using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Contracts
{
    public interface IOrderManager
    {
        Task<int> CreateOrderAsync(string userId, ICollection<CartItem> cartItems);
        Task<string?> ValidateCartItemsAsync(ICollection<CartItem> cartItems);

            Task<ICollection<Order>> GetUserOrdersAsync(string userId);
            Task<Order?> GetOrderByIdAsync(int orderId);
        Task UpdateOrderAsync(Order order);
          Task<ICollection<Order>> GetOrderswithDetails();



    }
}
