using Microsoft.AspNetCore.Identity;
using MVC_MyMusicStore.Models.UserModels;
namespace MVC_MyMusicStore.Data
{
    public class DbSeeder
    {
        public static async Task SeedData(IServiceProvider service)
        {
            var userMng = service.GetRequiredService<UserManager<IdentityUser>>();  
            var roleMng = service.GetRequiredService<RoleManager<IdentityRole>>();

            await roleMng.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await userMng.CreateAsync(new IdentityUser(Roles.User.ToString()));


            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = false,
            };

            var userInDb = await userMng.FindByEmailAsync(admin.Email);

            if(userInDb is null)
            {
                await userMng.CreateAsync(admin, "Admin@123");
                await userMng.AddToRoleAsync(admin ,Roles.Admin.ToString());
            }
        }
    }
}
