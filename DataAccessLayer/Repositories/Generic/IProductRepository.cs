using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Generic
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllPhones();
        public Task<List<Product>> GetAllWatches();
        public Task<string> GetCategoryByid(int id);
        public Task<List<Product>> GetProductsByIdsAsync(List<int> productIds);
        public Task<List<Product>> GetAllsearchbyname(string searchTerm = null, int? categoryId = null);

        public Task<List<Product>> GetAllProducts();
        public Task UpdateAsync(Product product);





    }
}
