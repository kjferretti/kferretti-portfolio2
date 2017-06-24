using kferretti_portfolio2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;
using System.Linq;

namespace kferretti_portfolio2.Migrations
{
    public class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }

            var userManager = new UserManager<ApplicationUser>(
     new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "kjferretti@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "kjferretti@gmail.com",
                    Email = "kjferretti@gmail.com",
                    FirstName = "Kevin",
                    LastName = "Ferretti",
                    DisplayName = "KevinTheBoss"
                }, "password12345");
            }

            if (!context.Users.Any(u => u.Email == "kjferret@ncsu.edu"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "mjaang@coderfoundry.com",
                    Email = "mjaang@coderfoundry.com",
                    FirstName = "Mark",
                    LastName = "Jaang",
                    DisplayName = "MarkTheMan"
                }, "password");
            }

            var userId = userManager.FindByEmail("kjferretti@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            var userId2 = userManager.FindByEmail("mjaang@coderfoundry.com").Id;
            userManager.AddToRole(userId2, "Moderator");
        }
    }
}