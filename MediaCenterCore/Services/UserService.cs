using MediaCenterCore.Models;
using MediaCenterCore.Shared;
using MediaCenterCore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MediaCenterCore.Services
{
    public class UserService
    {
        public UserService(UserManager<User> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<User> UserManager { get; }

        public async Task<OperationResult<ProfileVM>> GetUserInfo(string userId)
        {
           var user = await UserManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                return new OperationResult<ProfileVM>
                {
                    IsSuccess = true,
                    Object = new ProfileVM() { Email = user.Email, FullName = user.FullName, PhoneNumber = user.PhoneNumber }
                };
            }
            return new OperationResult<ProfileVM>
            {
                Message = "User not found",
                IsSuccess = false
            };
           
        }
    }
}
