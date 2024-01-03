/******************************************************************************
 *
 * File: AccountsController.cs
 *
 * Description: AccountsController.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelAdminApplication.DTOs;
using HotelModels;
using HotelModels.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HotelAdminApplication.Controllers
{

    public class AccountsController : Controller
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly UserManager<CustomerUser> _userManager;
        private readonly IUserStore<CustomerUser> _userStore;
        private readonly IUserEmailStore<CustomerUser> _emailStore;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountsController(
            ILogger<AccountsController> logger,
            UserManager<CustomerUser> userManager,
            IUserStore<CustomerUser> userStore,
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _roleManager = roleManager;
        }

        // GET: Accounts
        /// <summary>
        /// Indices the.
        /// </summary>
        /// <returns>A Task.</returns>
        [Authorize(Roles = Constants.AdministratorsRole)]
        public async Task<IActionResult> Index()
        {
            var usersWithRoles = _userManager.Users
            .Select(user => new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                Surname = user.Surname,
                Email = user.Email,
                ID_code = user.ID_code,
                PhoneNumber = user.PhoneNumber,

                Role = string.Join(", ", _userManager.GetRolesAsync(user).Result)
            })
            .ToList();
            return View(usersWithRoles);
        }

        // GET: Accounts/Register
        /// <summary>
        /// Registers the.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        [Authorize(Roles = Constants.AdministratorsRole)]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Accounts/Register
        /// <summary>
        /// Registers the.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A Task.</returns>
        [Authorize(Roles = Constants.AdministratorsRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new CustomerUser(model.Email, model.FirstName, model.Surname, model.ID_code, model.PhoneNumber);

                user.EmailConfirmed = true;

                await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"User '{model.Email}' created a new account with password.");

                    if (model.Role == Constants.ManagersRole || model.Role == Constants.AdministratorsRole)
                    {
                        await EnsureRole(user, model.Role);
                    }

                    return RedirectToAction("Index");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        // GET: Accounts/Details
        /// <summary>
        /// Details the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        [Authorize(Roles = $"{Constants.AdministratorsRole},{Constants.ManagersRole}")]
        public async Task<IActionResult> Details(string? id)
        {
            var user = _userManager.Users.Where(u=>u.Id==id)
           .Select(user => new UserDto()
           {
               Id = user.Id,
               UserName = user.UserName,
               FirstName = user.FirstName,
               Surname = user.Surname,
               Email = user.Email,
               ID_code = user.ID_code,
               PhoneNumber = user.PhoneNumber,

              // Role = string.Join(", ", _userManager.GetRolesAsync(user).Result)
           }).FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }


        /// <summary>
        /// Gets the email store.
        /// </summary>
        /// <returns>An IUserEmailStore.</returns>
        private IUserEmailStore<CustomerUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<CustomerUser>)_userStore;
        }

        /// <summary>
        /// Ensures the role.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The role.</param>
        /// <returns>A Task.</returns>
        private async Task<IdentityResult> EnsureRole(CustomerUser user, string role)
        {
            if (_roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await _roleManager.RoleExistsAsync(role))
            {
                IR = await _roleManager.CreateAsync(new IdentityRole(role));
            }

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await _userManager.AddToRoleAsync(user, role);

            return IR;
        }

    }
}
