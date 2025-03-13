using BusinessLogicLayer.Contracts;
using DataAccessLayer.Entities;
using Ecommerce_App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
namespace Ecommerce_App.Controllers
{ [Authorize]  // تأكد إن المستخدم مسجل دخول
       

    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartManager _manager;
        private readonly IProductManager _productManager;
        public ShoppingCartController(IShoppingCartManager manager, IProductManager productManager)
        {
            _manager = manager;
            _productManager = productManager;
        }

        // عرض محتوى السلة
        public async Task<IActionResult> ShowCart()
        {
                var cart = GetCartFromCookies();

                if (cart.CartItems == null || !cart.CartItems.Any())
                    cart.CartItems = new List<CartItem>();

                var productIds = cart.CartItems.Select(i => i.ProductId).ToList();
                var products = await _productManager.GetProductsByIdsAsync(productIds);

                var items = cart.CartItems.Select(item =>
                {
                    var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                    return new CartItemDetailsViewModel
                    {
                        ProductId = item.ProductId,
                        ProductName = product?.Name,
                        ImageUrl = product?.ImageUrl,
                        Price = product?.Price ?? 0,
                        Quantity = item.Quantity
                    };
                }).ToList();

                var viewModel = new ShoppingCartViewModel
                {
                    CartItems = items
                };

                return View(viewModel);
            

        }


        // إضافة منتج للسلة
        [HttpPost]
        public IActionResult AddToCart(AddToCartViewModel addToCartView)
        {
            var cart = GetCartFromCookies();
            if (cart.CartItems == null)
                cart.CartItems = new List<CartItem>();
            var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == addToCartView.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += addToCartView.Quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = addToCartView.ProductId,
                    Quantity = addToCartView.Quantity
                });
            }

            SaveCartToCookies(cart);
            return RedirectToAction("ShowCart");
        }

        // إزالة عنصر من السلة
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCartFromCookies();

            var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                cart.CartItems.Remove(item);
                SaveCartToCookies(cart);
            }

            return RedirectToAction("ShowCart");
        }
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCartFromCookies();

            if (cart.CartItems == null)
                cart.CartItems = new List<CartItem>();

            var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                // لو الكمية الجديدة أقل من أو تساوي صفر، نحذف العنصر من السلة
                if (quantity <= 0)
                {
                    cart.CartItems.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }

                SaveCartToCookies(cart);
            }

            return RedirectToAction("ShowCart");
        }


        // استرجاع الكارت من الكوكيز
        private ShoppingCart GetCartFromCookies()
        {
            var cartJson = Request.Cookies["ShoppingCart"];
            if (!string.IsNullOrEmpty(cartJson))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cartJson) ?? new ShoppingCart();
            }

            return new ShoppingCart();
        }

        // حفظ الكارت في الكوكيز لمدة 30 دقيقة
        private void SaveCartToCookies(ShoppingCart cart)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30),
                HttpOnly = true,
                IsEssential = true
            };

            var cartJson = JsonSerializer.Serialize(cart);
            Response.Cookies.Append("ShoppingCart", cartJson, options);
        }
    } }

