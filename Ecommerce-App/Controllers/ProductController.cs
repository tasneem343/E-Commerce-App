using BusinessLogicLayer.Contracts;
using BusinessLogicLayer.DTOs.Product;
using BusinessLogicLayer.Managers;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Generic;
using Ecommerce_App.ActionRequests;
using Ecommerce_App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecommerce_App.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductManager _productManager;
        private readonly ICategoryManager _categoryManager;

        private readonly IFileservice _fileService;
        public ProductController(IProductManager productManager, IFileservice fileService, ICategoryManager categoryManager)
        {
            _productManager = productManager;
            _fileService = fileService;
            _categoryManager = categoryManager;

        }
        public async Task< IActionResult> Index(string searchTerm)
        {
            List<GetorUpdateproductDTO> products;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                products = await _productManager.GetAllsearchbyname(searchTerm);
            }
            else
            {
                products = await _productManager.GetAllAsync();
            }

            return View(products);
        }
        
        public async Task<IActionResult> ShowAllPhones()
        {

            var products = await _productManager.GetAllPhones();

            return View("ShowAll", products);
        }
        public async Task<IActionResult> ShowAllWatches()
        {

            var products = await _productManager.GetAllWatches();

            return View("ShowAllWatches", products);
        }

        [Authorize(Roles = "Admin,Buyer")]

        [HttpGet]

        public async Task<IActionResult> Add()
        {
            ViewBag.Categories = await _categoryManager.GetAllAsync();

            return View("Add");
        }
        [HttpPost]
        public async Task<IActionResult> SaveAdd(CreateProductActionRequest product)
        {
            if (ModelState.IsValid)
            {
                var uniqueFileName = _fileService.UploadFile(product.Image, "Images");


                var productDto = new CreateProductDTO
                {
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    ImageUrl = uniqueFileName,
                    Stock = +1,
                    CategoryId = product.CategoryId


                };
                await _productManager.AddAsync(productDto);


                return RedirectToAction(nameof(Add));
            }

            var departments = await _productManager.GetAllAsync();
            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> Details( int id)
        {

            var product =await _productManager.GetByIdAsync(id);
            if (product == null) throw new Exception("Product not found");


            var productvm = new GetOrUpdateProductViewModel
            {
                Price = product.Price,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ProductId=product.ProductId,
                CategoryName = await _productManager.GetCategoryByid(id)


            };
            

                return View("Details",productvm);
           

        }
 

    }
    
}
