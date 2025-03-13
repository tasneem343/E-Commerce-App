using BusinessLogicLayer.Contracts;
using BusinessLogicLayer.DTOs.Product;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Generic;
using DataAccessLayer.Repositories.IUnitWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Managers
{
    public class ProductManager :IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;


        public ProductManager(IUnitOfWork unitOfWork,IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }
        public async Task<IEnumerable<GetorUpdateproductDTO>> GetAllPhones()
        {
            IEnumerable<Product> result = await _productRepository.GetAllPhones();
            var productdtolist = result.Select(p => new GetorUpdateproductDTO
            {
                Price = p.Price,
                ProductId = p.ProductId,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Stock = p.Stock,
            }).ToList();
            return productdtolist;
        }
        public async Task<IEnumerable<GetorUpdateproductDTO>> GetAllWatches()
        {
            IEnumerable<Product> result = await _productRepository.GetAllWatches();
            var productdtolist = result.Select(p => new GetorUpdateproductDTO
            {
                Price = p.Price,
                ProductId = p.ProductId,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Stock = p.Stock,
            }).ToList();


            return productdtolist;
        }
        
        public async Task<List<GetorUpdateproductDTO>> GetAllAsync()
        {
                var products=await _unitOfWork.Products.GetAllAsync();
            var productdtolist= products.Select(p=>new GetorUpdateproductDTO
            {
                Price = p.Price,
                ProductId = p.ProductId,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Stock = p.Stock,
            }).ToList();

            
            return productdtolist;
        }

        public async Task<GetorUpdateproductDTO> GetByIdAsync(int id)
        { var product=await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) throw new Exception("Product not found");
            var productdto = new GetorUpdateproductDTO
            {
                Price = product.Price,
                ProductId = product.ProductId,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Stock = product.Stock
            };
            return productdto;

        }
        public async Task<string> GetCategoryByid(int id)
        {
            var categoryname = await _productRepository.GetCategoryByid(id);
            return categoryname;
     

        }

        public async Task AddAsync(CreateProductDTO productdto)
        {
            var product = new Product
            {
              Price= productdto.Price,
              Description= productdto.Description,
              ImageUrl = productdto.ImageUrl,
              Name = productdto.Name,
              Stock = productdto.Stock,
              CategoryId=productdto.CategoryId
              

            };
            _unitOfWork.Products.AddAsync(product);
             await _unitOfWork.CompleteAsync();
            

        }

        public async Task UpdateAsync(GetorUpdateproductDTO product)
        {
            var prod = new Product
            {
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Stock = product.Stock,

            };
            _unitOfWork.Products?.UpdateAsync(prod);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product != null)
            {
                _unitOfWork.Products?.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
            }
        }
        public async Task<List<Product>> GetProductsByIdsAsync(List<int> productIds)
        {
            return await _productRepository.GetProductsByIdsAsync(productIds);
                
        }
        public async Task<List<GetorUpdateproductDTO>> GetAllsearchbyname(string searchTerm = null)
        {
            var products = await _productRepository.GetAllsearchbyname(searchTerm);
            var productdtolist = products.Select(p => new GetorUpdateproductDTO
            {
                Price = p.Price,
                ProductId = p.ProductId,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Stock = p.Stock,
            }).ToList();


            return productdtolist;
        }

    }
}


