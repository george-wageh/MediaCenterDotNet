using MediaCenterCore.IRepositories;
using MediaCenterCore.Models;
using MediaCenterCore.Shared;
using MediaCenterCore.ViewModels;
using MediaCenterCore.Shared;

namespace MediaCenterCore.Services
{
    public class CategoryService
    {
        public CategoryService(ICategoriesRepository categoriesRepository , UnitAppWork unitAppWork)
        {
            CategoriesRepository = categoriesRepository;
            UnitAppWork = unitAppWork;
        }

        public ICategoriesRepository CategoriesRepository { get; }
        public UnitAppWork UnitAppWork { get; }
        public async Task<OperationResult<CategoryVM>> GetAsync(int id) {
            var category = await CategoriesRepository.Get(id);
            if (category == null) {
                return new OperationResult<CategoryVM>
                {
                    IsSuccess = false,
                    Message = "Category not found",
                };
            }
            var vm = new CategoryVM { 
                Id = id,
                Name = category.Name,
            };
            return new OperationResult<CategoryVM>
            {
                IsSuccess = true,
                Object = vm
            };
        }
        public async Task<OperationResult<CategoryVM>> AddAsync(CategoryVM categoryVM) {
            var category = new Category
            {
                Name = categoryVM.Name
            };
            await CategoriesRepository.AddAsync(category);
            await UnitAppWork.SaveChanges();
            categoryVM.Id = category.Id;
            return new OperationResult<CategoryVM>
            {
                IsSuccess = true,
                Message = "",
                Object = categoryVM
            };
        }

        public async Task<OperationResult<object>> DeleteAsync(int categoryId) {
            var category = await CategoriesRepository.Get(categoryId);
            if (category != null) {
                CategoriesRepository.Delete(category);
                await UnitAppWork.SaveChanges();
            }
            return new OperationResult<object>
            {
                IsSuccess = true,
                Message = "Category Removed Successfully",
            };
        }
        public async Task<OperationResult<IEnumerable<CategoryVM>>> GetAllAsync() { 
            var Categories =  await CategoriesRepository.GetAllAsync();
            var CategoriesVM = Categories.Select(x => new CategoryVM
            {
                Id = x.Id,
                Name = x.Name
            });
            return new OperationResult<IEnumerable<CategoryVM>>
            {
                IsSuccess = true,
                Message = "",
                Object = CategoriesVM
            };
        }

        public async Task<IEnumerable<CategoryVM>> GetAllInMediaAsync(int mediaId) {
            var Categories = await CategoriesRepository.GetAllInMediaAsync(mediaId);
            var CategoriesVM = Categories.Select(x => new CategoryVM
            {
                Id = x.Id,
                Name = x.Name
            });
            return CategoriesVM;
        }
        public async Task<OperationResult<object>> UpdateAsync(CategoryVM categoryVM) {
            var category = new Category
            {
                Id= categoryVM.Id,
                Name = categoryVM.Name
            };
            CategoriesRepository.Update(category);
            await UnitAppWork.SaveChanges();
            return new OperationResult<object> { 
                IsSuccess= true,
                Message="",
                Object = category
            };
        }

    }
}
