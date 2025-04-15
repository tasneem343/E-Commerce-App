using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Generic
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
            
        }
        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }
        public async Task<List<Product>> GetAllPhones()
        {
           List<Product> result= await _context.Products.Where(p=>p.CategoryId==1).ToListAsync();
            return result;
        }
        public async Task<List<Product>> GetAllWatches()
        {
            List<Product> result = await _context.Products.Where(p => p.CategoryId == 2).ToListAsync();
            return result;
        }
        public async Task< string> GetCategoryByid(int id)
        {
            var result = await _context.Products.Include(a => a.Category).FirstOrDefaultAsync(p => p.ProductId==id);
            return result.Category.Name;
        }
        public async Task<List<Product>> GetProductsByIdsAsync(List<int> productIds)
        {
            return await _context.Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToListAsync();
        }
        public async Task<List<Product>> GetAllsearchbyname(string searchTerm = null, int? categoryId = null)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchTerm));
            }

            if (categoryId != null && categoryId != 0)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            return await query.ToListAsync();
        }
        public async Task UpdateAsync( Product product)
        {
          var prod= await  _context.Products.FirstOrDefaultAsync(p => p.ProductId == product.ProductId);
            if(prod == null) throw new Exception("Product not found");
            prod.Name = product.Name;
            prod.ProductId = product.ProductId;
            prod.Price = product.Price;
            prod.Description = product.Description;
            prod.ImageUrl = product.ImageUrl;
            prod.Stock = product.Stock;
            prod.CategoryId = product.CategoryId;
            await _context.SaveChangesAsync();
        }


    }
}
