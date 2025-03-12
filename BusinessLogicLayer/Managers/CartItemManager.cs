using BusinessLogicLayer.Contracts;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.IUnitWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Managers
{
    public class CartItemManager:ICartItemManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartItemManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync()
        {
            return await _unitOfWork.CartItem.GetAllAsync();
        }

        public async Task<CartItem> GetByIdAsync(int id)
        {
            return await _unitOfWork.CartItem.GetByIdAsync(id);
        }

        public async Task AddAsync(CartItem cartItem)
        {
            await _unitOfWork.CartItem.AddAsync(cartItem);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(CartItem cartItem)
        {
            await _unitOfWork.CartItem.UpdateAsync(cartItem);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.CartItem.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId)
        {
            var allItems = await _unitOfWork.CartItem.GetAllAsync();
            return allItems.Where(ci => ci.ShoppingCartId == cartId);
        }
        public async Task AddOrUpdateCartItemAsync(string userId, int productId, int quantity)
        {
            var cart = await _unitOfWork.ShoppingCarts
                .GetAllAsync();
            var userCart = cart.FirstOrDefault(c => c.UserId == userId);

            if (userCart == null)
            {
                userCart = new ShoppingCart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                await _unitOfWork.ShoppingCarts.AddAsync(userCart);
                await _unitOfWork.CompleteAsync();
            }

            var cartItems = await _unitOfWork.CartItem.GetAllAsync();
            var existingItem = cartItems.FirstOrDefault(ci => ci.ShoppingCartId == userCart.ShoppingCartId && ci.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                await _unitOfWork.CartItem.UpdateAsync(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    ShoppingCartId = userCart.ShoppingCartId,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _unitOfWork.CartItem.AddAsync(newItem);
            }

            await _unitOfWork.CompleteAsync();
        }

    }
}

