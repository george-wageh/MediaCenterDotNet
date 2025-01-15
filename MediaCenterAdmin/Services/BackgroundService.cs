using MediaCenterCore.Data;
using MediaCenterCore.Models;
using Microsoft.AspNetCore.Identity;

namespace MediaCenterAdmin.Services
{
    public class BackgroundServiceOnRun : IHostedService
    {
        public BackgroundServiceOnRun(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var dbContext = scope.ServiceProvider.GetRequiredService<MediaCenterDbContext>();

                // Ensure roles exist
                var roles = new[] { "user", "admin", "owner" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole { Name = role });
                    }
                }
                var owner = await userManager.FindByEmailAsync("20201700211@cis.asu.edu.eg");
                if (owner == null) {
                    owner = new User
                    {
                        Email = "20201700211@cis.asu.edu.eg",
                        FullName = "George wageh rizk",
                        PhoneNumber = "01201254611",
                        UserName = Guid.NewGuid().ToString(),
                    };
                    await userManager.CreateAsync(owner , "*******************");
                    await userManager.AddToRolesAsync(owner, roles);
                    await dbContext.SaveChangesAsync();
                }
               
            }
        }



        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Cleanup logic here
            Console.WriteLine("Server shutting down...");
            return Task.CompletedTask;
        }
    }
}
