using MediaCenterCore.Models;

namespace MediaCenterCore.IRepositories
{
    public interface IWatchLaterRepository
    {
        public Task AddAsync(MediaWatchLater mediaWatchLater);
        public void Remove(MediaWatchLater mediaWatchLater);
        public Task<bool> IsExisit(int mediaId, string userId);
        public Task<IEnumerable<Media>> GetAllAsync(string userId);
        public Task<MediaWatchLater> GetAsync(int mediaId , string userId);
    }
}
