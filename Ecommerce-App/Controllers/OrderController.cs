using BusinessLogicLayer.Contracts;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;
using System.Text.Json;

namespace Ecommerce_App.Controllers
{
    [Authorize]

    public class OrderController : Controller
    {
        private readonly IOrderManager _orderManager;

        public OrderController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        //payment
        public async Task<IActionResult> PayWithStripe(int id)
        {
            var order = await _orderManager.GetOrderByIdAsync(id);
            if (order == null) return NotFound();

            var domain = "https://localhost:7165"; 

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(order.TotalAmount * 100), 
                    Currency = "USD",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = $"Order #{order.OrderId}"
                    }
                },
                Quantity = 1
            }
        },
                Mode = "payment",
                SuccessUrl = $"{domain}/Order/PaymentSuccess?id={order.OrderId}",
                CancelUrl = $"{domain}/Order/PaymentCancel?id={order.OrderId}"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303); // Redirect
        }
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

            //  Validate stock before creating order
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
            return RedirectToAction("singleOrderDetails", new { id = orderId });
        }
        public async Task<IActionResult> PaymentSuccess(int id)
        {
            var order = await _orderManager.GetOrderByIdAsync(id);
            if (order != null)
            {
                order.Status = OrderStatus.Completed;
                await _orderManager.UpdateOrderAsync(order);
            }

            TempData["Message"] = "Payment Successed";
            return RedirectToAction("singleOrderDetails", new { id = id });
        }

        public async Task<IActionResult> PaymentCancel(int id)
        {
            var order = await _orderManager.GetOrderByIdAsync(id);
            if (order != null)
            {
                order.Status = OrderStatus.Canceled;
                await _orderManager.UpdateOrderAsync(order);
            }

            TempData["Message"] = "payment is cancelled";
            return RedirectToAction("singleOrderDetails", routeValues: new { id = id });
        }


        public async Task<IActionResult> singleOrderDetails(int id)
        {
            var order = await _orderManager.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return View("OrderDetails",order);
        }
        [Authorize(Roles = "Admin,Customer")]
        [HttpPost]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var order = await _orderManager.GetOrderByIdAsync(id);
            if (order == null) return NotFound();

            if (order.Status == OrderStatus.Pending)
            {
                order.Status = OrderStatus.Canceled;
                await _orderManager.UpdateOrderAsync(order);
                TempData["Message"] = "Order has been canceled successfully.";
            }
            else
            {
                TempData["Message"] = "Only pending orders can be canceled.";
            }

            return RedirectToAction("singleOrderDetails", routeValues: new { id = id });
        }

        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderManager.GetUserOrdersAsync(userId);
            return View(orders);
        }
        [Authorize(Roles = "Buyer,Admin")]
        public async Task<IActionResult> OrdersDetails()
        {
            var orders = await _orderManager.GetOrderswithDetails();
          
            return View("MyOrders",orders);
        }
    }
}
