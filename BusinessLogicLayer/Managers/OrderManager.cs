using BusinessLogicLayer.Contracts;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Generic;
using DataAccessLayer.Repositories.IUnitWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Managers
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductManager _productManager;
        private readonly IOrderRepository _orderRepository;

        public OrderManager(IUnitOfWork unitOfWork, IProductManager productManager, IOrderRepository orderRepository)
        {
            _unitOfWork = unitOfWork;
            _productManager = productManager;
            _orderRepository = orderRepository;

        }

        public async Task<int> CreateOrderAsync(string userId, ICollection<CartItem> cartItems)
        {
            // التحقق أولاً من توافر الكمية لكل منتج
            foreach (var item in cartItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new InvalidOperationException($"Product with ID {item.ProductId} not found.");

                if (product.Stock < item.Quantity)
                    throw new InvalidOperationException($"The product '{product.Name}' does not have enough stock.");
            }

            // إذا كل المنتجات متاحة نبدأ في إنشاء الطلب
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                OrderItems = new List<OrderItem>()
            };

            decimal total = 0;

            foreach (var item in cartItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);

                // خصم الكمية
                product.Stock -= item.Quantity;

                var orderItem = new OrderItem
                {
                    ProductId = product.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                };

                total += product.Price * item.Quantity;
                order.OrderItems.Add(orderItem);
            }
            order.TotalAmount = total;

            _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            return order.OrderId;
        }

        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            return await _orderRepository.GetAllOrdersWithDetailsAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {

            return await _orderRepository.GetOrderByIdAsync(orderId);
        }
        
            
        public async Task<string?> ValidateCartItemsAsync(ICollection<CartItem> cartItems)
        {
            foreach (var item in cartItems)
            {
                var product = await _productManager.GetByIdAsync(item.ProductId);
                if (product == null)
                    return "One or more products do not exist.";

                if (product.Stock < item.Quantity)
                    return $"The requested quantity of '{product.Name}' is not available.";
            }

            return null;
        }

    } }
