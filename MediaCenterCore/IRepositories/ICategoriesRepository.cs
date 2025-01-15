using MediaCenterCore.Data;
using MediaCenterCore.Models;

namespace MediaCenterCore.IRepositories
{
    public interface ICategoriesRepository
    {
        public Task AddAsync(Category category);
        public Task<Category> Get(int categoryId);
        public void Delete(Category category);
        public void Update(Category category);
        public Task<IEnumerable<Category>> GetAllAsync();
        public Task<IEnumerable<Category>> GetAllInMediaAsync(int mediaId);
    }
}
