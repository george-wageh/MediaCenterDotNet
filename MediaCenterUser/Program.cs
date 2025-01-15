

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MediaCenterCore.Data;
using MediaCenterCore.Models;

using MediaCenterCore.Services;
using MediaCenterCore.IRepositories;
using MediaCenterCore.Repositories;
using MediaCenterCore.Shared;
using MediaCenterCore.Abstractions;
using MediaCenterCore.Extensions;
using MediaCenterUser.Services;

namespace MediaCenterUser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<MediaCenterDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            });
            builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<MediaCenterDbContext>().AddDefaultTokenProviders();

            // Add services from the shared library
            builder.Services.AddSharedLibraryServices();

            builder.Services.AddScoped<IWebHostEnvironmentAccessor, WebHostEnvironmentAccessor>();


            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                 options.LoginPath = "/Account/Login";
            });

            //builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}