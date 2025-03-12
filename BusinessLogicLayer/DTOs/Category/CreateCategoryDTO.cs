using BusinessLogicLayer.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; }
        public ICollection<CreateProductDTO> ?Products { get; set; }
    }
}
