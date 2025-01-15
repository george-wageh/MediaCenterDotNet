using Microsoft.EntityFrameworkCore;
using MediaCenterCore.Data;
using MediaCenterCore.IRepositories;
using MediaCenterCore.Models;
using MediaCenterCore.Shared;

namespace MediaCenterCore.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        public MediaCenterDbContext MediaCenterDbContext { get; }
        public UnitAppWork UnitAppWork { get; }

        public MediaRepository(MediaCenterDbContext mediaCenterDbContext, UnitAppWork unitAppWork) {
            MediaCenterDbContext = mediaCenterDbContext;
            UnitAppWork = unitAppWork;
        }

        public async Task<int> AddAsync(Media media)
        {
            await MediaCenterDbContext.AddAsync(media);
            await UnitAppWork.SaveChanges();
            return media.Id;
        }

        public async Task UpdateAsync(Media media)
        {
            MediaCenterDbContext.Update(media);
            await MediaCenterDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Media>> GetAllAsync(string? q, int? categoryId)
        {
            return await MediaCenterDbContext.Set<Media>()
                 .Include(x => x.MediaCategories)
                 .Where(x => x.Name.Contains(q ?? "")).Where(x => categoryId == null ||
                              x.MediaCategories.Any(y => y.CategoryId == categoryId))
                 .ToListAsync();
        }

        public async Task<Media> GetAsync(int mediaId)
        {
            return await MediaCenterDbContext.Set<Media>().Include(x => x.MediaCategories).FirstOrDefaultAsync(x => x.Id == mediaId);
        }

        public void Remove(Media media)
        {
            MediaCenterDbContext.Set<Media>().Remove(media);
        }


        public void AddCategoriesToMedia(Media media,IEnumerable<int> categoryIds) {
            var Categories = categoryIds.Select(x => new MediaCategory
            {
                MediaId = media.Id,
                CategoryId = x
            }).ToList();
            media.MediaCategories = Categories;
        }


    }
}
