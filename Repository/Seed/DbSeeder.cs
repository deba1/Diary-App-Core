using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Seed
{
    public static class DbSeeder
    {
        public static void SeedSettings(this ModelBuilder builder)
        {
            SettingName[] settings = (SettingName[])Enum.GetValues(typeof(SettingName));

            foreach (var setting in settings)
            {
                builder.Entity<Setting>().HasData(new Setting
                {
                    Id = setting.ToString(),
                    Status = 1
                });
            }
        }

        public static void SeedUsers(this ModelBuilder builder)
        {
            var ph = new PasswordHasher<IdentityUser>();
            var admin = new IdentityUser
            {
                Id = "1",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.admin",
                EmailConfirmed = true
            };
            admin.PasswordHash = ph.HashPassword(admin, "admin");

            builder.Entity<IdentityUser>().HasData(admin);
        }

        public static void SeedRoles(this ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "admin",
                    Name = "Admin",
                    NormalizedName = "admin"
                },
                new IdentityRole
                {
                    Id = "regular",
                    Name = "Regular",
                    NormalizedName = "regular"
                }
            );
        }

        public static void SeedUserRoles(this ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "admin",
                    UserId = "1"
                }
            );
        }
    }
}
