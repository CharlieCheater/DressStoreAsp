using DressStore.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DressStore.Infrastructure.Data
{
    public class DbInitializer
    {
        private const string _userName = "GlobalAdmin";
        public static async Task InitAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationContext>();
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();
            
            var hasRoles = await roleManager.Roles.AnyAsync();
            if(!hasRoles)
            {
                await AddRoles(roleManager);
            }
            var hasAdmin = (await userManager.FindByNameAsync(_userName)) != null;
            if(hasAdmin)
            {
                return;
            }
            ApplicationUser user = new ApplicationUser()
            {
                UserName = _userName,
                FirstName = "Временный администратор",
                LastName = "Временный администратор",
                Email = "globalAdmin@dressshop.com"
            };
            var result = await userManager.CreateAsync(user, "qwerty");
            await userManager.AddToRoleAsync(user, "Admin");
            if(result.Succeeded)
            {
                Debug.WriteLine("Юзер создан");
            }

        }

        private static async Task AddRoles(RoleManager<ApplicationRole> roleManager)
        {
            await roleManager.CreateAsync(new ApplicationRole() { Name = "User", NormalizedName = "USER" });
            await roleManager.CreateAsync(new ApplicationRole() { Name = "Admin", NormalizedName = "ADMIN" });
        }
    }
}
