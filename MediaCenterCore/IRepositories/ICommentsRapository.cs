using MediaCenterCore.Models;

namespace MediaCenterCore.IRepositories
{
    public interface ICommentsRapository
    {
        public Task AddAsync(MediaComment mediaComment);
        public void Remove(MediaComment mediaComment);
        public Task<IEnumerable<MediaComment>> GetAllAsync(int mediaId);
        public Task<MediaComment> GetAsync(int commentId);

    }
}
