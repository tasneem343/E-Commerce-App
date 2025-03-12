using BusinessLogicLayer.Contracts;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce_App.Controllers
{ [Authorize]  // تأكد إن المستخدم مسجل دخول
        public class ShoppingCartController : Controller
        {
            private readonly IShoppingCartManager _shoppingCartManager;
            private readonly ICartItemManager _cartItemManager;

            public ShoppingCartController(IShoppingCartManager shoppingCartManager, ICartItemManager cartItemManager)
            {
                _shoppingCartManager = shoppingCartManager;
                _cartItemManager = cartItemManager;
            }

            // عرض محتوى السلة
            public async Task<IActionResult> ShowCart()
            {
                var userId = GetUserId();
                var cart = await _shoppingCartManager.GetCartByUserIdAsync(userId);
                return View(cart);
            }

        // إضافة منتج للسلة
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var userId = GetUserId();

            // تأكد من وجود السلة
            var cart = await _shoppingCartManager.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = userId
                };
                await _shoppingCartManager.AddCartAsync(cart);
            }

            // أضف العنصر للسلة
            await _cartItemManager.AddOrUpdateCartItemAsync(userId, productId, quantity);
            return RedirectToAction("Index");
        }

        // إزالة عنصر من السلة
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
            {
                await _cartItemManager.DeleteAsync(cartItemId);
                return RedirectToAction("Index");
            }

            // مساعد للحصول على الـ UserId من الـ Claims
            private string GetUserId()
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }
    }

