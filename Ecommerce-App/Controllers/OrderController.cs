using BusinessLogicLayer.Contracts;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Ecommerce_App.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderManager _orderManager;

        public OrderController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        [Authorize]
        [Authorize]
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var cartJson = Request.Cookies["ShoppingCart"];
            if (string.IsNullOrEmpty(cartJson))
            {
                TempData["Message"] = "Cart is empty.";
                return RedirectToAction("ShowCart", "ShoppingCart");
            }

            var cart = JsonSerializer.Deserialize<ShoppingCart>(cartJson) ?? new ShoppingCart();
            if (cart.CartItems == null || !cart.CartItems.Any())
            {
                TempData["Message"] = "Cart is empty.";
                return RedirectToAction("ShowCart", "ShoppingCart");
            }

            // ✅ Validate stock before creating order
            var validationMessage = await _orderManager.ValidateCartItemsAsync(cart.CartItems);
            if (validationMessage != null)
            {
                TempData["Message"] = validationMessage;
                return RedirectToAction("ShowCart", "ShoppingCart");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orderId = await _orderManager.CreateOrderAsync(userId, cart.CartItems);

            // Clear the cart
            Response.Cookies.Delete("ShoppingCart");

            TempData["Message"] = "Order placed successfully!";
            return RedirectToAction("OrderDetails", new { id = orderId });
        }


        [Authorize]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _orderManager.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return View(order);
        }

        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderManager.GetUserOrdersAsync(userId);
            return View(orders);
        }
    }
}
