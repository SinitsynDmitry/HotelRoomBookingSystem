/******************************************************************************
 *
 * File: SeedData.cs
 *
 * Description: SeedData.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/


using HotelModels;
using HotelModels.Helpers;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace HotelAdminApplication.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            //using (var context = new ApplicationDbContext(
            //    serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                //Passwords must have at least one non alphanumeric character.
                //Passwords must have at least one lowercase('a' - 'z').
                //Passwords must have at least one uppercase('A' - 'Z').

                // The admin user can do anything
                var admin = await EnsureUser(serviceProvider, testUserPw, "admin@hotels.com", "Admin");
                await EnsureRole(serviceProvider, admin, Constants.AdministratorsRole);

                // allowed user can create and edit rooms
                var manager = await EnsureUser(serviceProvider, testUserPw, "manager@hotels.com", "Manager");
                await EnsureRole(serviceProvider, manager, Constants.ManagersRole);

                //SeedDB(context, adminID);
            }
        }

        private static async Task<CustomerUser> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string email, string userName)
        {
            var userManager = serviceProvider.GetService<UserManager<CustomerUser>>();
            var userStore = serviceProvider.GetService<IUserStore<CustomerUser>>();
           var emailStore=(IUserEmailStore<CustomerUser>)userStore;

            var user = await userManager.FindByNameAsync(email);
            if (user == null)
            {
                user = new CustomerUser
                {
                    UserName = email,
                    FirstName = userName,
                    Surname = userName,
                    Email = email,
                    EmailConfirmed = true
                };
                await userStore.SetUserNameAsync(user, email, CancellationToken.None);
                await emailStore.SetEmailAsync(user, email, CancellationToken.None);
                var result = await userManager.CreateAsync(user, testUserPw);

                if (!result.Succeeded)
                {
                    if (result.Errors?.Count() > 0)
                    {
                        var error = string.Join(",", result.Errors.Select(e => e.Description));
                        throw new Exception(error);
                    }

                    throw new Exception("The password is probably not strong enough!");
                }

            }

            return user;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,CustomerUser user, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<CustomerUser>>();

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
    }
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
