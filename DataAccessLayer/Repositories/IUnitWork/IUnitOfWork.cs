using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IUnitWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Order> Orders { get; }
         IGenericRepository<CartItem> CartItem { get; }

        IGenericRepository<OrderItem> OrderItems { get; }
        IGenericRepository<ShoppingCart> ShoppingCarts { get; }
        Task<int> CompleteAsync(); //  save changes 
    }
}
