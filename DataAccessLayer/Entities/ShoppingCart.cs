﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class ShoppingCart
    {
       
            public int ShoppingCartId { get; set; }
            public string UserId { get; set; }
            public ApplicationUser User { get; set; }
            public ICollection<CartItem> CartItems { get; set; }
        
    }
}
