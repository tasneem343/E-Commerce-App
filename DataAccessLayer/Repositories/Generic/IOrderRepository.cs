using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Generic
{
   public interface IOrderRepository
    {
        public  Task<List<Order>> GetAllOrdersWithDetailsAsync();
        public Task<Order> GetOrderByIdAsync(int orderId);


    }
}
