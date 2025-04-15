using BusinessLogicLayer.Contracts;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Products;
using BusinessLogicLayer.Managers;
using BusinessLogicLayer.Services;
using Ecommerce_App.ActionRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_App.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryManager _categoryManager;
        public CategoryController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }
    
        [Authorize(Roles = "Admin")]

        [HttpGet]

        public IActionResult Add()
        {
            return View("Add");
        }
        [HttpPost]
        public async Task<IActionResult> SaveAdd(CreateCategoryActionRequest category)
        {
            if (ModelState.IsValid)
            {
                var categorydto = new CreateCategoryDTO
                {
                    Name = category.Name,
                  
                };
                await _categoryManager.AddAsync(categorydto);


                return RedirectToAction(nameof(Add));
            }

            var catogries = await _categoryManager.GetAllAsync();
            return View(category);
        }


    }
}

