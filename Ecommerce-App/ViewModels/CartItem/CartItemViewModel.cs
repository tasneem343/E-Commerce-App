using DataAccessLayer.Entities;

namespace Ecommerce_App.ViewModels.CartItem
{
    public class CartItemViewModel
    {
       
            public int CartItemId { get; set; }
            public int ShoppingCartId { get; set; }
            public ShoppingCart ShoppingCart { get; set; }
            public int ProductId { get; set; }
            public Product Product { get; set; }
            public int Quantity { get; set; }
        
    }
}
