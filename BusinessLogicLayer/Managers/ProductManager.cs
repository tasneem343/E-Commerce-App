using BusinessLogicLayer.Contracts;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.IUnitWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Managers
{
    public class ProductManager 
    {
        private readonly IUnitOfWork _UnitIfWork;
        //public Task AddAsync(Product product)
        //{
            
        //}

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
