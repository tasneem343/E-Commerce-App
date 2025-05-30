﻿using BusinessLogicLayer.DTOs.Products;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Contracts
{
    public interface IProductManager
    {
      
            Task<List<GetorUpdateproductDTO>> GetAllAsync();
            Task<GetorUpdateproductDTO> GetByIdAsync(int id);
            Task AddAsync(CreateProductDTO product);
            Task UpdateAsync(GetorUpdateproductDTO product);
            Task DeleteAsync(int id);

        public Task<IEnumerable<GetorUpdateproductDTO>> GetAllPhones();
        public  Task<IEnumerable<GetorUpdateproductDTO>> GetAllWatches();
        public Task<string> GetCategoryByid(int id);
        public Task<List<Product>> GetProductsByIdsAsync(List<int> productIds);
        public  Task<List<GetorUpdateproductDTO>> GetAllsearchbyname(string searchTerm = null, int? categoryId = null);









    }
}
