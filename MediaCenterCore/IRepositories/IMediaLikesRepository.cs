using MediaCenterCore.Models;

namespace MediaCenterCore.IRepositories
{
    public interface IMediaLikesRepository
    {
        public Task AddAsync(MediaLike mediaLike);
        public void Remove(MediaLike mediaLike);
        public Task<MediaLike> GetMediaLike(int mediaId, string userId);
    }
}
