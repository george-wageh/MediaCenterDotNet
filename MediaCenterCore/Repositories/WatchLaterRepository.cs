
using MediaCenterCore.IRepositories;
using MediaCenterCore.Models;
using Microsoft.EntityFrameworkCore;
using MediaCenterCore.Data;

namespace MediaCenterCore.Repositories
{
    public class WatchLaterRepository : IWatchLaterRepository
    {
        public MediaCenterDbContext MediaCenterDbContext { get; }

        public WatchLaterRepository(MediaCenterDbContext mediaCenterDbContext)
        {
            MediaCenterDbContext = mediaCenterDbContext;
        }

        public async Task AddAsync(MediaWatchLater mediaWatchLater)
        {
            await MediaCenterDbContext.Set<MediaWatchLater>().AddAsync(mediaWatchLater);
        }

        public async Task<IEnumerable<Media>> GetAllAsync(string userId)
        {
            return await MediaCenterDbContext.Set<MediaWatchLater>().Where(x => x.User.Id == userId).Include(x=>x.Media).Select(x=>x.Media).ToListAsync();
        }

        public async Task<bool> IsExisit(int mediaId, string userId)
        {
            return await MediaCenterDbContext.Set<MediaWatchLater>().AnyAsync(x => x.MediaId == mediaId && x.UserId == userId);
        }

        public void Remove(MediaWatchLater mediaWatchLater)
        {
            MediaCenterDbContext.Set<MediaWatchLater>().Remove(mediaWatchLater);
        }

        public Task<MediaWatchLater> GetAsync(int mediaId, string userId)
        {
            return MediaCenterDbContext.Set<MediaWatchLater>().FirstOrDefaultAsync(x => x.MediaId == mediaId && x.UserId == userId);
        }
    }
}
