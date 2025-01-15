using MediaCenterCore.Data;
using MediaCenterCore.IRepositories;
using MediaCenterCore.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaCenterCore.Repositories
{
    public class MediaLikesRepository:IMediaLikesRepository
    {
        public MediaLikesRepository(MediaCenterDbContext  mediaCenterDbContext)
        {
            MediaCenterDbContext = mediaCenterDbContext;
        }

        public MediaCenterDbContext MediaCenterDbContext { get; }
        public async Task AddAsync(MediaLike mediaLike) 
        {
           await MediaCenterDbContext.Set<MediaLike>().AddAsync(mediaLike);
        }

        public void Remove(MediaLike mediaLike)
        {
           MediaCenterDbContext.Set<MediaLike>().Remove(mediaLike);
        }

        public Task<MediaLike> GetMediaLike(int mediaId , string userId)
        {
            return MediaCenterDbContext.Set<MediaLike>().FirstOrDefaultAsync(x => x.MediaId == mediaId && x.UserId == userId);
        }


    }
}
