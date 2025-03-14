﻿namespace Ecommerce_App.ViewModels
{
    public class GetOrUpdateProductViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Stock { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }

    }
}
