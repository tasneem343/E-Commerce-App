using BusinessLogicLayer.DTOs.Products;

namespace Ecommerce_App.ActionRequests
{
    public class CreateCategoryActionRequest
    {
        public string Name { get; set; }
        public ICollection<CreateProductDTO>? Products { get; set; }
    }
}
