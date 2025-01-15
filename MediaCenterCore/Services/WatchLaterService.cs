using MediaCenterCore.IRepositories;
using MediaCenterCore.Models;
using MediaCenterCore.Shared;
using MediaCenterCore.ViewModels;

namespace MediaCenterCore.Services
{
    public class WatchLaterService
    {
        public WatchLaterService(IWatchLaterRepository watchLaterRepository , UnitAppWork unitAppWork)
        {
            WatchLaterRepository = watchLaterRepository;
            UnitAppWork = unitAppWork;
        }

        public IWatchLaterRepository WatchLaterRepository { get; }
        public UnitAppWork UnitAppWork { get; }
        string ImagesPath = Shared.DomainsNames.HostStaticDomain + "/media-images/";

        public async Task<IEnumerable<MediaListVM>> GetAllAsync(string userId) { 
            var medias = await WatchLaterRepository.GetAllAsync(userId);
            return medias.Select(media => new MediaListVM
            {
                Name = media.Name,
                Id = media.Id,
                Image = ImagesPath + "/" + media.Image,
                Actors = (new StaffC(media.Staff).GetActorsAsString())
            }).ToList();
        }
        public async Task<OperationResult<object>> AddWatchLater(int mediaId, string userId)
        {
            if (!(await WatchLaterRepository.IsExisit(mediaId, userId)))
            {
                var MediaWatchLater = new MediaWatchLater
                {
                    MediaId = mediaId,
                    UserId = userId
                };
                await WatchLaterRepository.AddAsync(MediaWatchLater);
                await UnitAppWork.SaveChanges();
            }
            return new OperationResult<object>
            {
                IsSuccess = true
            };
        }
        public async Task<OperationResult<object>> RemoveWatchLater(int mediaId, string userId) {
           var mediaWatchLater = await WatchLaterRepository.GetAsync(mediaId, userId);
            if(mediaWatchLater != null) {
                WatchLaterRepository.Remove(mediaWatchLater);
                await UnitAppWork.SaveChanges();
            }
            return new OperationResult<object>
            {
                IsSuccess = true
            };
        }

        public async Task<bool?> IsWatchLater(int mediaId, string userId) {
            return await WatchLaterRepository.IsExisit(mediaId, userId);
        }


    }
}
