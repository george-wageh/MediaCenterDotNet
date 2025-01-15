using MediaCenterCore.IRepositories;
using MediaCenterCore.Models;
using MediaCenterCore.Shared;

namespace MediaCenterCore.Services
{
    public class MediaLikesService
    {
        public MediaLikesService(IMediaLikesRepository mediaLikesRepository, UnitAppWork unitAppWork)
        {
            MediaLikesRepository = mediaLikesRepository;
            UnitAppWork = unitAppWork;
        }

        public IMediaLikesRepository MediaLikesRepository { get; }
        public UnitAppWork UnitAppWork { get; }

        public async Task<OperationResult<object>> ToggleLikeAsync(int mediaId, string userId)
        {
            var mediaLike = await MediaLikesRepository.GetMediaLike(mediaId, userId);
            if (mediaLike == null)
            {
                mediaLike = new MediaLike()
                {
                    IsLike = true,
                    UserId = userId,
                    MediaId = mediaId
                };
                await MediaLikesRepository.AddAsync(mediaLike);
                await UnitAppWork.SaveChanges();
            }
            else {
                mediaLike.IsLike = !mediaLike.IsLike;
                await UnitAppWork.SaveChanges();
            }
            return new OperationResult<object>
            {
                IsSuccess = true,
                Object = null
            };
        }

        public async Task<bool?> MediaIsLike(int mediaId , string userId) {
            var mediaLike = await MediaLikesRepository.GetMediaLike(mediaId, userId);
            if (mediaLike == null) return null;
            return mediaLike.IsLike;
        }

    }
}
