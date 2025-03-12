using BusinessLogicLayer.Contracts;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_App.Controllers
{
    public class CartItemController : Controller
    {
        private readonly ICartItemManager _cartItemManager;

        public CartItemController(ICartItemManager cartItemManager)
        {
            _cartItemManager = cartItemManager;
        }

        // عرض كل العناصر
        public async Task<IActionResult> Index()
        {
            var items = await _cartItemManager.GetAllAsync();
            return View(items);
        }

        // تفاصيل عنصر واحد
        public async Task<IActionResult> Details(int id)
        {
            var item = await _cartItemManager.GetByIdAsync(id);
            if (item == null)
                return NotFound();
            return View(item);
        }

        // إنشاء عنصر جديد
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CartItem item)
        {
            if (ModelState.IsValid)
            {
                await _cartItemManager.AddAsync(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // تعديل عنصر
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _cartItemManager.GetByIdAsync(id);
            if (item == null)
                return NotFound();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CartItem item)
        {
            if (ModelState.IsValid)
            {
                await _cartItemManager.UpdateAsync(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // حذف
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _cartItemManager.GetByIdAsync(id);
            if (item == null)
                return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _cartItemManager.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
