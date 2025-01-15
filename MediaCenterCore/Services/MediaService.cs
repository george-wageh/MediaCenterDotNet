using MediaCenterCore.IRepositories;
using MediaCenterCore.Models;
using MediaCenterCore.Shared;
using MediaCenterCore.ViewModels;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using MediaCenterCore.Abstractions;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MediaCenterCore.Services
{
    public class MediaService
    {
        public MediaService(IMediaRepository mediaRepository, IWebHostEnvironmentAccessor WebHostEnvironment, CommentsService commentsService,
            FavoritesService favoritesService, WatchLaterService watchLaterService ,MediaLikesService mediaLikesService, CategoryService CategoryService, MediaVAService mediaVAService, UnitAppWork unitAppWork)
        {
            MediaRepository = mediaRepository;
            this.WebHostEnvironment = WebHostEnvironment;
            CommentsService = commentsService;
            FavoritesService = favoritesService;
            WatchLaterService = watchLaterService;
            MediaLikesService = mediaLikesService;
            this.CategoryService = CategoryService;
            MediaVAService = mediaVAService;
            UnitAppWork = unitAppWork;
        }
        string ImagesPath = Shared.DomainsNames.HostStaticDomain + "/media-images/";
        public IMediaRepository MediaRepository { get; }
        public IWebHostEnvironmentAccessor WebHostEnvironment { get; }
        public CommentsService CommentsService { get; }
        public FavoritesService FavoritesService { get; }
        public WatchLaterService WatchLaterService { get; }
        public MediaLikesService MediaLikesService { get; }
        public CategoryService CategoryService { get; }
        public MediaVAService MediaVAService { get; }
        public UnitAppWork UnitAppWork { get; }

        public async Task<OperationResult<MediaDetailsVM>> GetDetailsAsync(int mediaId , string userId) {
            var media = await MediaRepository.GetAsync(mediaId);
            if (media != null) {

                var Comments = await CommentsService.GetAllInMediaAsync(mediaId);

                var IsLike = await MediaLikesService.MediaIsLike(mediaId, userId);
                var IsWatchLater = await WatchLaterService.IsWatchLater(mediaId, userId);
                var IsFav = await FavoritesService.IsFav(mediaId, userId);

                var jsonString = media.Staff;
                var jsonObject = JsonSerializer.Deserialize<Dictionary<string, ICollection<string>>>(jsonString);

                var mediaVM = new MediaDetailsVM
                {
                    Country = media.Country,
                    Name = media.Name,
                    Description = media.Description,
                    Id = media.Id,
                    Image = ImagesPath + "/" + media.Image,
                    Language = media.Language,
                    Quality = media.Quality,
                    ReleaseDate = media.ReleaseDate,
                    Actors = jsonObject["Actors"],
                    Directors = jsonObject["Directors"],
                    Comments = Comments
                };
                mediaVM.IsLike = IsLike;
                mediaVM.IsFav = IsFav;
                mediaVM.IsWatchLater = IsWatchLater;
                return new OperationResult<MediaDetailsVM>
                {
                    IsSuccess = true,
                    Message = "",
                    Object = mediaVM
                };
            }
            return new OperationResult<MediaDetailsVM>
            {
                IsSuccess = false,
                Message = "Media not found",
            };

        }
        public async Task<OperationResult<ViewMediaVM>> GetViewMediaVMAsync(int mediaId) {
            var media = await MediaRepository.GetAsync(mediaId);
            if (media == null)
            {
                return new OperationResult<ViewMediaVM>
                {
                    IsSuccess = false,
                    Message = "Meida not found",
                };
            }
            StaffC staffC = new StaffC(media.Staff);
            var Comments = await CommentsService.GetAllInMediaAsync(mediaId);
            var fileName = await MediaVAService.GetFileName(mediaId);
            fileName = fileName != null ? MediaCenterCore.Shared.DomainsNames.HostStaticDomain + "/media-va" + "/" + fileName : null;
            var mediaVm = new ViewMediaVM
            {
                Image = ImagesPath + "/" + media.Image,
                Actors = staffC.Actors,
                Directors = staffC.Directors,
                Id = media.Id,
                Name = media.Name,
                Comments = Comments,
                FileName = fileName
            };
            return new OperationResult<ViewMediaVM>
            {
                IsSuccess = true,
                Message = "",
                Object = mediaVm
            };
        }
        public async Task<OperationResult<MediaVM>> GetMediaVmAsync(int mediaId) { 
            var media = await MediaRepository.GetAsync(mediaId);
            if (media == null) {
                return new OperationResult<MediaVM>
                {
                    IsSuccess = false,
                    Message = "Meida not found",
                };
            }
            StaffC staffC = new StaffC(media.Staff);
            var Categories = await CategoryService.GetAllInMediaAsync(mediaId);
            var mediaVm = new MediaVM
            {
                Image = ImagesPath +"/"+ media.Image,
                Actors = staffC.Actors,
                Directors = staffC.Directors,
                Country = media.Country,
                Id = media.Id,
                Description = media.Description,
                Language = media.Language,
                Name = media.Name,
                Quality = media.Quality,
                Categories = Categories
            };
            return new OperationResult<MediaVM>
            {
                IsSuccess = true,
                Message = "",
                Object = mediaVm
            };
        }
        public async Task<OperationResult<IEnumerable<MediaListVM>>> Query(string? q, int? categoryId)
        {
            var medias = await MediaRepository.GetAllAsync(q, categoryId);
            var MediasVM = medias.Select(x => new MediaListVM
            {
                Id = x.Id,
                Name = x.Name,
                Image = ImagesPath + "/" + x.Image,
                Actors = (new StaffC(x.Staff).GetActorsAsString())
            }).ToList();
            return new OperationResult<IEnumerable<MediaListVM>>
            {
                IsSuccess = true,
                Object = MediasVM,
                Message = ""
            };
        }

        public async Task<OperationResult<string>> UploadImage(IFormFile formFile)
        {
            var url = Shared.DomainsNames.HostStaticDomain + "/UploadImage";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var content = new MultipartFormDataContent();
                    var fileContent = new StreamContent(formFile.OpenReadStream()); // 10MB limit
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(formFile.ContentType);
                    content.Add(fileContent, "file", formFile.FileName);

                    // Send the POST request
                    var response = await httpClient.PostAsync(url, content);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseContent = await response.Content.ReadFromJsonAsync<OperationResult<string>>();
                        if (responseContent.IsSuccess)
                        {
                            return new OperationResult<string>
                            {
                                IsSuccess = true,
                                Object = responseContent.Object
                            };
                        }
                    }
                    return new OperationResult<string>
                    {
                        IsSuccess = false,
                        Message = $"Upload failed with status code {response.StatusCode}",
                        Object = null
                    };
                   
                }
                catch (Exception e)
                {
                    return new OperationResult<string>
                    {
                        IsSuccess = false,
                        Message = $"Exception: {e.Message}",
                        Object = null
                    };
                }
            }

            //var fileName = Path.GetRandomFileName() + Path.GetExtension(formFile.FileName);

            //var filePath = Path.Combine(WebHostEnvironment.WebRootPath, "media-images", fileName);

            //using (var fileStream = new FileStream(filePath, FileMode.Create))
            //{
            //    try
            //    {
            //        await formFile.CopyToAsync(fileStream);
            //        return new OperationResult<string>
            //        {
            //            IsSuccess = true,
            //            Object = fileName

            //        };
            //    }
            //    catch (Exception e)
            //    {
            //        return new OperationResult<string>
            //        {
            //            Message = $"Exception {e.InnerException.Message}",
            //            IsSuccess = false,
            //            Object = null
            //        };
            //    }

            //}
        }
        public async Task<Media> MapToMedia(MediaVM mediaVM , Media media) {
            var jsonStaff = (new StaffC(mediaVM.Actors, mediaVM.Directors)).GetJsonString();

            media.Country = mediaVM.Country;
            media.Description = mediaVM.Description;
            media.Language = mediaVM.Language;
            media.Name = mediaVM.Name;
            media.Quality = mediaVM.Quality;
            media.Staff = jsonStaff;
            media.MediaCategories = mediaVM.Categories?
                .Select(category => new MediaCategory
                {
                    CategoryId = category.Id
                })
            .ToList();
            if (mediaVM.Photo != null && mediaVM.Photo.Length > 0)
            {
                var uploadStatus = await UploadImage(mediaVM.Photo);
                if (uploadStatus.IsSuccess)
                {
                    media.Image = uploadStatus.Object;
                }
            }
            return media;
        }

        public async Task<OperationResult<object>> EditAsync(int id, MediaVM mediaVM , int photoFlag) {
            var oldMedia = await MediaRepository.GetAsync(id);
            if (oldMedia != null)
            {
                var media = await MapToMedia(mediaVM, oldMedia);
                media.Id = id;
                if (photoFlag == 0)
                {
                    media.Image = "no-image.png";
                }
                await MediaRepository.UpdateAsync(media);
                return new OperationResult<object>
                {
                    IsSuccess = true
                };
            }
            else {
                return new OperationResult<object>
                {
                    IsSuccess = false,
                    Message = "Media not found"
                };
            }
        }

        public async Task<OperationResult<int>> AddAsync(MediaVM mediaVM) {
            var media = await MapToMedia(mediaVM , new Media());
            var mediaId = await MediaRepository.AddAsync(media);
            return new OperationResult<int>
            {
                IsSuccess = true,
                Object = mediaId
            };
        }

        public async Task<OperationResult<object>> ToggleLikeAsync(int mediaId,string userId) {
            return await MediaLikesService.ToggleLikeAsync(mediaId, userId);
        }

        public async Task<OperationResult<MediaListVM>> GetMediaCard(int id) { 
            var media = await MediaRepository.GetAsync(id);
            if (media != null) {
                return new OperationResult<MediaListVM>
                {
                    IsSuccess = true,
                    Message = "",
                    Object = new MediaListVM
                    {
                        Id = media.Id,
                        Name = media.Name,
                        Image = ImagesPath + media.Image,
                        Actors = (new StaffC(media.Staff).GetActorsAsString())
                    }
                };
            }
            return new OperationResult<MediaListVM>
            {
                IsSuccess = false,
                Message = "Media not found"
            };
        }

        public async Task<OperationResult<object>> DeleteAsync(int mediaId) {
            var media = await MediaRepository.GetAsync(mediaId);
            if(media != null) {
                MediaRepository.Remove(media);
                await UnitAppWork.SaveChanges();
                return new OperationResult<object>
                {
                    IsSuccess = true,
                    Message = "Media removed successfully"
                };
            }
            return new OperationResult<object>
            {
                IsSuccess = false,
                Message = "Media not found"
            };
        }

    }
}
