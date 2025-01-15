using MediaCenterCore.Data;
using MediaCenterCore.IRepositories;
using MediaCenterCore.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaCenterCore.Repositories
{
    public class FavoritesRepository:IFavoritesRepository
    {
        public FavoritesRepository(MediaCenterDbContext mediaCenterDbContext) {
            MediaCenterDbContext = mediaCenterDbContext;
        }

        public MediaCenterDbContext MediaCenterDbContext { get; }

        public async Task AddAsync(MediaAddFav mediaFav) { 
            await MediaCenterDbContext.Set<MediaAddFav>().AddAsync(mediaFav);
        }

        public void Remove(MediaAddFav mediaFav)
        {
            MediaCenterDbContext.Set<MediaAddFav>().Remove(mediaFav);
        }

        public async Task<bool> IsExisit(int mediaId, string userId) {
            return await MediaCenterDbContext.Set<MediaAddFav>().AnyAsync(x => x.MediaId == mediaId && x.UserId == userId);
        }

        public async Task<IEnumerable<Media>> GetAllAsync(string userId) { 
             return await MediaCenterDbContext.Set<MediaAddFav>().Where(x=>x.User.Id == userId).Include(x=>x.Media).Select(x=>x.Media).ToListAsync();
        }
        public async Task<MediaAddFav> GetAsync(int mediaId,string userId)
        {
            return await MediaCenterDbContext.Set<MediaAddFav>().FirstOrDefaultAsync(x => x.User.Id == userId && x.MediaId == mediaId);
        }
    }
}
