using MediaCenterCore.IRepositories;
using MediaCenterCore.Models;
using MediaCenterCore.Shared;
using MediaCenterCore.ViewModels;

namespace MediaCenterCore.Services
{
    public class FavoritesService
    {
        public FavoritesService(IFavoritesRepository favoritesRepository , UnitAppWork unitAppWork)
        {
            FavoritesRepository = favoritesRepository;
            UnitAppWork = unitAppWork;
        }

        public IFavoritesRepository FavoritesRepository { get; }
        public UnitAppWork UnitAppWork { get; }
        string ImagesPath = Shared.DomainsNames.HostStaticDomain + "/media-images/";

        public async Task<IEnumerable<MediaListVM>> GetAllAsync(string userId)
        {
            var medias = await FavoritesRepository.GetAllAsync(userId);
            return medias.Select(media => new MediaListVM
            {
                Name = media.Name,
                Id = media.Id,
                Image = ImagesPath + "/" + media.Image,
                Actors = (new StaffC(media.Staff).GetActorsAsString())
            }).ToList();
        }
        public async Task<OperationResult<object>> AddFav(int mediaId, string userId)
        {
            if (!(await FavoritesRepository.IsExisit(mediaId, userId)))
            {
                var MediaAddFav = new MediaAddFav
                {
                    MediaId = mediaId,
                    UserId = userId
                };
                await FavoritesRepository.AddAsync(MediaAddFav);
                await UnitAppWork.SaveChanges();
            }
            return new OperationResult<object>
            {
                IsSuccess = true
            };
        }
        public async Task<OperationResult<object>> RemoveFav(int mediaId, string userId)
        {
            var mediafav = await FavoritesRepository.GetAsync(mediaId, userId);
            if (mediafav != null)
            {
                FavoritesRepository.Remove(mediafav);
                await UnitAppWork.SaveChanges();
            }
            return new OperationResult<object>
            {
                IsSuccess = true
            };
        }

        public async Task<bool?> IsFav(int mediaId, string userId)
        {
            return await FavoritesRepository.IsExisit(mediaId, userId);
        }
    }
}
