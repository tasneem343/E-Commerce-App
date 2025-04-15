using BusinessLogicLayer.DTOs.Category;
using BusinessLogicLayer.DTOs.Products;
using DataAccessLayer.Entities;

namespace Ecommerce_App.ViewModels
{
    public class ProductSearchViewModel
    {
        public List<GetorUpdateproductDTO> Products { get; set; }
        public List<GetOrUpdateCategoryDTO>? Catgories { get; set; }

        public string SearchTerm { get; set; }
        public int? SelectedCategoryId { get; set; }
    }
}
