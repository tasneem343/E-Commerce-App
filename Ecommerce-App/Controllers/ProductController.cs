using BusinessLogicLayer.Contracts;
using BusinessLogicLayer.DTOs.Products;
using Ecommerce_App.ActionRequests;
using Ecommerce_App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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
        public async Task<IActionResult> Index(string searchTerm, int categoryId)
        {
            var categories = await _categoryManager.GetAllAsync();

            List<GetorUpdateproductDTO> products;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                products = await _productManager.GetAllsearchbyname(searchTerm, categoryId);
            }
            else
            {
                products = await _productManager.GetAllAsync();
            }

            var viewModel = new ProductSearchViewModel
            {
                Products = products,
                SearchTerm = searchTerm,
                SelectedCategoryId = categoryId,
                Catgories = categories.ToList()
            };

        

            return View(viewModel);
        }
        public async Task<IActionResult> ShowAll()
        {
            var products = await _productManager.GetAllAsync();
            return View("ShowAll", products);
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
                var prod =await _productManager.GetAllsearchbyname(product.Name);
                foreach(var p in prod)
                {

                    if (p.Name.ToLower() == product.Name.ToLower())
                    {
                        if (p.Price == product.Price)
                        {
                            
                            p.Stock += 1;
                            await _productManager.UpdateAsync(p);
                        }
                        else
                        {
                            p.Price = product.Price;
                            p.Stock += 1;

                            await _productManager.UpdateAsync(p);
                        }
                    }
                    return RedirectToAction(nameof(Index), controllerName: "Home");
                }
                var productDto = new CreateProductDTO
                {
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    ImageUrl = uniqueFileName,
                    Stock = 1,
                    CategoryId = product.CategoryId


                };
                await _productManager.AddAsync(productDto);


                return RedirectToAction(nameof(Index),controllerName:"Home");
            }

            var products = await _productManager.GetAllAsync();
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
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _productManager.DeleteAsync(id);
            return RedirectToAction(nameof(ShowAll));
        }
        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productManager.GetByIdAsync(id);
            if (product == null) throw new Exception("Product not found");

            var ProductAR = new UpdateProductsActionRequest
            {
                Price = product.Price,
                Name = product.Name,
                Description = product.Description,
                
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ProductId = product.ProductId,
                CategoryName = await _productManager.GetCategoryByid(id)
            };
            ViewBag.Categories = await _categoryManager.GetAllAsync();
            return View("Edit", ProductAR);
        }
        [HttpPost]
        public async Task<IActionResult> SaveEdit(UpdateProductsActionRequest productFromRequest)
        {

            var productdto= await _productManager.GetByIdAsync(productFromRequest.ProductId);
            if (productdto == null) throw new Exception("Product not found");
            if (ModelState.IsValid)
            {
                 productdto = new GetorUpdateproductDTO
                {
                    ProductId = productFromRequest.ProductId,
                    Price = productFromRequest.Price,
                    Name = productFromRequest.Name,
                    Description = productFromRequest.Description,
                    ImageUrl = productdto.ImageUrl,
                    Stock = productFromRequest.Stock,
                    CategoryId = productFromRequest.CategoryId,
                    CategoryName = productFromRequest.CategoryName

                };
               await _productManager.UpdateAsync(productdto);
                
                
                return RedirectToAction(nameof(ShowAll));
            }
            
            return View("Edit", productFromRequest);
        }

    }
    
}
