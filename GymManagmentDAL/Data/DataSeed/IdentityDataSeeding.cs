using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Data.DataSeed
{
    public static class IdentityDataSeeding
    {
        public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {
                if (!roleManager.Roles.Any())
                {
                    var role = new List<IdentityRole>()
                    {
                        new IdentityRole
                        {
                            Name = "SuperAdmin",
                        },
                        new IdentityRole
                        {
                            Name = "Admin",
                        }
                    };
                    foreach (var r in role)
                    {
                        if (!roleManager.RoleExistsAsync(r.Name).Result)
                        {
                            roleManager.CreateAsync(r).Wait();
                        }
                    }
                }
                if (!userManager.Users.Any())
                {
                    var superAdmin = new ApplicationUser
                    {
                        FirstName = "Ahmed",
                        LastName = "Reda",
                        UserName = "AhmedReda",
                        Email = "ahmed@gmail.com",
                        PhoneNumber = "1234567890",
                    };
                    userManager.CreateAsync(superAdmin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(superAdmin, "SuperAdmin").Wait();
                    var Admin = new ApplicationUser
                    {
                        FirstName = "Mahmoud",
                        LastName = "Elrefaay",
                        UserName = "MahmoudElrefaay",
                        Email = "mahmoud@gmail.com",
                        PhoneNumber = "523697411"
                    };
                    userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred during seeding: {ex.Message}");
                return false;
            } 


        }
    }
}
