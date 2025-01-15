using MediaCenterCore.Data;
using MediaCenterCore.IRepositories;
using MediaCenterCore.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaCenterCore.Repositories
{
    public class CommentsRapository : ICommentsRapository
    {
        public CommentsRapository(MediaCenterDbContext mediaCenterDbContext)
        {
            MediaCenterDbContext = mediaCenterDbContext;
        }

        public MediaCenterDbContext MediaCenterDbContext { get; }

        public async Task AddAsync(MediaComment mediaComment)
        {
           await MediaCenterDbContext.Set<MediaComment>().AddAsync(mediaComment);
        }

        public async Task<IEnumerable<MediaComment>> GetAllAsync(int mediaId)
        {
            return await MediaCenterDbContext.Set<MediaComment>().Where(x => x.MediaId == mediaId).ToListAsync();
        }

        public async Task<MediaComment> GetAsync(int commentId)
        {
            return await MediaCenterDbContext.Set<MediaComment>().FirstOrDefaultAsync(x => x.Id == commentId);
        }

        public void Remove(MediaComment mediaComment)
        {
            MediaCenterDbContext.Set<MediaComment>().Remove(mediaComment);
        }
    }
}
