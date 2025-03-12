using BusinessLogicLayer.DTOs.Product;

namespace Ecommerce_App.ActionRequests
{
    public class CreateCategoryActionRequest
    {
        public string Name { get; set; }
        public ICollection<CreateProductDTO>? Products { get; set; }
    }
}
