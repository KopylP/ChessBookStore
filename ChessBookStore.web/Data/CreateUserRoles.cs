using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChessBookStore.web.Models;
namespace ChessBookStore.web.Data
{
    public static class CreateUserRoles
    {
        public static async Task Create(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();

            IdentityResult roleResult;
            //Adding Admin Role
            var roleCheckAdmin = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheckAdmin)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole() { Name = "Admin", NormalizedName = "ADMIN"});
            }
            var roleCheckUser = await RoleManager.RoleExistsAsync("User");
            if(!roleCheckUser)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole() { Name = "User", NormalizedName = "USER" });
            }
            //Assign Admin role to the main User here we have given our newly registered 
            //login id for Admin management
            User user = await UserManager.FindByEmailAsync("p.kopyl9@gmail.com");
            await UserManager.AddToRoleAsync(user, "ADMIN");
            await UserManager.AddToRoleAsync(user, "USER");
        }
    }

}
