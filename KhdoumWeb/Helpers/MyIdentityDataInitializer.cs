using KhdoumWeb.Data;
using KhdoumWeb.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhdoumWeb.Helpers
{
    public static class MyIdentityDataInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager)
        {
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            try
            {
                if (userManager.FindByNameAsync("GhJ5KB3sM1laGnk5@gmail.com").Result == null)
                {
                    ApplicationUser user = new ApplicationUser();
                    user.UserName = "GhJ5KB3sM1laGnk5@gmail.com";
                    user.Email = "GhJ5KB3sM1laGnk5@gmail.com";
                    user.EmailConfirmed = true;
                    //user.FullName = "Nancy Davolio";
                    //user.BirthDate = new DateTime(1960, 1, 1);

                    IdentityResult result = userManager.CreateAsync
                    (user, "AAHjjyuKK@%$%jKl&Fh@1Tyo5C$vVb88R%").Result;

                    if (result.Succeeded)
                    {

                    }
                }
            }
            catch
            {

            }
            
        }

        public static void SeedRoles(ApplicationDbContext context)
        {
            try
            {
                context.Database.EnsureCreated();//if db is not exist ,it will create database .but ,do nothing .

                // Look for any students.
                if (context.Roless.Any())
                {
                    return;   // DB has been seeded
                }

                context.Roless.Add(new Role { Name = "Supplier" });
                context.Roless.Add(new Role { Name = "Customer" });
                context.SaveChanges();
            }
            catch
            {

            }

            

        }






    }
}
