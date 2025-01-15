using MediaCenterCore.Models;

namespace MediaCenterCore.IRepositories
{
    public interface IFavoritesRepository
    {
        public Task AddAsync(MediaAddFav mediaFav);
        public void Remove(MediaAddFav mediaFav);
        public Task<bool> IsExisit(int mediaId, string userId);
        public Task<IEnumerable<Media>> GetAllAsync(string userId);
        public Task<MediaAddFav> GetAsync(int mediaId, string userId);
    }
}
