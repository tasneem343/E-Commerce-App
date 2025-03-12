using BusinessLogicLayer.Contracts;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.IUnitWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessLogicLayer.Managers.ShoppingCartManager;

namespace BusinessLogicLayer.Managers
{

    public class ShoppingCartManager : IShoppingCartManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ShoppingCart>> GetAllCartsAsync()
        {
            return await _unitOfWork.ShoppingCarts.GetAllAsync();
        }

        public async Task<ShoppingCart> GetCartByIdAsync(int id)
        {
            return await _unitOfWork.ShoppingCarts.GetByIdAsync(id);
        }

        public async Task AddCartAsync(ShoppingCart cart)
        {
            await _unitOfWork.ShoppingCarts.AddAsync(cart);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateCartAsync(ShoppingCart cart)
        {
            await _unitOfWork.ShoppingCarts.UpdateAsync(cart);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteCartAsync(int id)
        {
            await _unitOfWork.ShoppingCarts.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<ShoppingCart> GetCartByUserIdAsync(string userId)
        {
            var carts = await _unitOfWork.ShoppingCarts.GetAllAsync();
            return carts.FirstOrDefault(c => c.UserId == userId);
        }

        public async Task AddItemToCartAsync(string userId, int productId, int quantity)
        {
            var cart = await GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                await _unitOfWork.ShoppingCarts.AddAsync(cart);
                await _unitOfWork.CompleteAsync();
            }

            var existingItem = cart.CartItems?.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    ShoppingCartId = cart.ShoppingCartId
                };

                if (cart.CartItems == null)
                    cart.CartItems = new List<CartItem>();

                cart.CartItems.Add(newItem);
            }

            await _unitOfWork.ShoppingCarts.UpdateAsync(cart);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveItemFromCartAsync(string userId, int productId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            if (cart != null && cart.CartItems != null)
            {
                var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
                if (item != null)
                {
                    cart.CartItems.Remove(item);
                    await _unitOfWork.ShoppingCarts.UpdateAsync(cart);
                    await _unitOfWork.CompleteAsync();
                }
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            if (cart != null && cart.CartItems != null)
            {
                cart.CartItems.Clear();
                await _unitOfWork.ShoppingCarts.UpdateAsync(cart);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
    
}
