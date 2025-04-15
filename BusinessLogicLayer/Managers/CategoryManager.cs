using BusinessLogicLayer.Contracts;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Category;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.IUnitWork;


namespace BusinessLogicLayer.Managers
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetOrUpdateCategoryDTO>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            var Categorydtolist = categories.Select(c => new GetOrUpdateCategoryDTO
            {
                CategoryId = c.CategoryId,
                Name = c.Name
               
             
            }).ToList();


            return Categorydtolist;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _unitOfWork.Categories.GetByIdAsync(id);
        }

        public async Task AddAsync(CreateCategoryDTO category)
        {
            var category2 = new Category
            {
                Name = category.Name,
                Products = category.Products?.Select(p => new Product
                {
                    Price = p.Price,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId
                }).ToList()



            };
           await _unitOfWork.Categories.AddAsync(category2);
            await _unitOfWork.CompleteAsync();


        }

        public async Task UpdateAsync(GetOrUpdateCategoryDTO category)
        {
            var categ = new Category
            {
                Name = category.Name,
                CategoryId = category.CategoryId,
                Products = category.Products.Select(p => new Product
                {
                    Price = p.Price,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Stock = p.Stock,
                    ProductId = p.ProductId,

                }).ToList()

            };
           await _unitOfWork.Categories.UpdateAsync(categ);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category != null)
            {
              await  _unitOfWork.Categories?.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
