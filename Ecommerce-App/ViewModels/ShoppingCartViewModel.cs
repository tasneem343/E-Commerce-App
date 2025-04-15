namespace Ecommerce_App.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartItemDetailsViewModel> CartItems { get; set; } = new List<CartItemDetailsViewModel>();

        public decimal Total => CartItems.Sum(i => i.Total);
    }
}
