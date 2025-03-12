using BusinessLogicLayer.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_App.ViewComponents
{
    public class ProductSliderViewComponent : ViewComponent
    {
        private readonly IProductManager _productService;

        public ProductSliderViewComponent(IProductManager productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _productService.GetAllPhones();
            return View(products); 
        }
    }
}
