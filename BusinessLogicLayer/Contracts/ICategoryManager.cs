using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Category;
using BusinessLogicLayer.DTOs.Products;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Contracts
{
    public interface ICategoryManager
    {
        Task<IEnumerable<GetOrUpdateCategoryDTO>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(CreateCategoryDTO category);
        Task UpdateAsync(GetOrUpdateCategoryDTO category);
        Task DeleteAsync(int id);

    }
}
