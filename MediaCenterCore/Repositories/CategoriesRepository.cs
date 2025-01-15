using MediaCenterCore.Data;
using MediaCenterCore.IRepositories;
using MediaCenterCore.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaCenterCore.Repositories
{
    public class CategoriesRepository: ICategoriesRepository
    {
        public CategoriesRepository(MediaCenterDbContext mediaCenterDbContext) {
            MediaCenterDbContext = mediaCenterDbContext;
        }

        public MediaCenterDbContext MediaCenterDbContext { get; }

        public async Task AddAsync(Category category) { 
            await MediaCenterDbContext.Set<Category>().AddAsync(category);
        }

        public void Delete(Category category) {
            MediaCenterDbContext.Set<Category>().Remove(category);
        }

        public async Task<Category> Get(int categoryId)
        {
            return await MediaCenterDbContext.Set<Category>().FirstOrDefaultAsync(x => x.Id == categoryId);
        }


        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await MediaCenterDbContext.Set<Category>().ToListAsync();
        }
        public async Task<IEnumerable<Category>> GetAllInMediaAsync(int mediaId)
        {
            return await MediaCenterDbContext.Set<MediaCategory>().Where(x=>x.MediaId == mediaId).Include(x=>x.Category).Select(x=>x.Category).ToListAsync();
        }

        public void Update(Category category)
        {
            MediaCenterDbContext.Update(category);
        }
    }
}
