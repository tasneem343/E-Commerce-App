using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs.Category
{
    public class GetOrUpdateCategoryDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<GetOrUpdateCategoryDTO> Products { get; set; }
    }
}
