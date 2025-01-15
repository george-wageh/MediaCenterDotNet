using Microsoft.Extensions.DependencyInjection;
using MediaCenterCore.IRepositories;
using MediaCenterCore.Repositories;
using MediaCenterCore.Services;
using MediaCenterCore.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCenterCore.Extensions
{
    public static class ServiceRegistration
    {
        public static void AddSharedLibraryServices(this IServiceCollection services)
        {
            // Repository registrations
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<ICommentsRapository, CommentsRapository>();
            services.AddScoped<IFavoritesRepository, FavoritesRepository>();
            services.AddScoped<IMediaLikesRepository, MediaLikesRepository>();
            services.AddScoped<IWatchLaterRepository, WatchLaterRepository>();

            // Service registrations
            services.AddScoped<CategoryService>();
            services.AddScoped<CommentsService>();
            services.AddScoped<FavoritesService>();
            services.AddScoped<MediaCategoryService>();
            services.AddScoped<MediaLikesService>();
            services.AddScoped<MediaService>();
            services.AddScoped<SeriesRelationService>();
            services.AddScoped<UserService>();
            services.AddScoped<WatchLaterService>();
            services.AddScoped<MediaVAService>();

            // Unit of Work
            services.AddScoped<UnitAppWork>();
        }
    }
}
