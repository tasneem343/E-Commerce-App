using BusinessLogicLayer.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_App.ViewComponents
{
    public class WatchesSliderViewComponent:ViewComponent
    {

        private readonly IProductManager _productService;

        public WatchesSliderViewComponent(IProductManager productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _productService.GetAllWatches();
            return View(products);
        }
    }
}

