using MediaCenterCore.Models;

namespace MediaCenterCore.IRepositories
{
    public interface IMediaRepository
    {
        public Task<IEnumerable<Media>> GetAllAsync(string? q, int? categoryId);
        public Task<Media> GetAsync(int mediaId);
        public Task<int> AddAsync(Media media);
        public void Remove(Media media);
        public void AddCategoriesToMedia(Media media, IEnumerable<int> categoryIds);
        public Task UpdateAsync(Media media);
    }
}
