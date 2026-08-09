using LibrarySystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Data.Seed
{
    public class DataSeed : IDataSeed
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeed(ApplicationDbContext dbContext
            , UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDataAsync()
        {
           
            if ((await _dbContext.Database.GetPendingMigrationsAsync()).Any())
            {
                await _dbContext.Database.MigrateAsync();
            }

            await SeedRolesAsync();
            await SeedAdminAsync();
        }
        private async Task SeedRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await _roleManager.RoleExistsAsync("Member"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Member"));
            }
        }
        private async Task SeedAdminAsync()
        {
            var email = "abdalrahman.hamdan129@gmail.com";
            var password = "Admin@123";
            ApplicationUser? adminUser = await _userManager.FindByEmailAsync(email);
            if (adminUser is null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "abdfahmawe",
                    Email = email,
                    FullName = "Abdalrahman Hamdan",
                    EmailConfirmed = true
                };
                IdentityResult result = await _userManager.CreateAsync(adminUser, password);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join(", ",
                            result.Errors.Select(error => error.Description)));
                }
            }
            if (!await _userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                IdentityResult roleResult =
                    await _userManager.AddToRoleAsync(adminUser, "Admin");

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join(", ",
                            roleResult.Errors.Select(error => error.Description)));
                }
            }
        }
   
    
    }
    
}
