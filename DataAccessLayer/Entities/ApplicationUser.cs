using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class ApplicationUser:IdentityUser
    {
        
            public string FullName { get; set; }
            public string Address { get; set; }
            public ICollection<Order>? Orders { get; set; }
        public string UserType { get; set; } 

       
       

    }
}
