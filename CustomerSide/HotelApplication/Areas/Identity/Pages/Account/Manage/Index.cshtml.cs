/******************************************************************************
 *
 * File: Index.cshtml.cs
 *
 * Description: Index.cshtml.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

#nullable disable

using HotelApplication.Models;
using HotelApplication.Validations;
using HotelModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HotelApplication.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<CustomerUser> _userManager;
        private readonly SignInManager<CustomerUser> _signInManager;

        public IndexModel(
            UserManager<CustomerUser> userManager,
            SignInManager<CustomerUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }


        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }


        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            /// <summary>
            /// Gets or sets the phone number.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            /// <summary>
            /// Gets or sets the first name.
            /// </summary>
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            /// <summary>
            /// Gets or sets the surname.
            /// </summary>
            [Required]
            [Display(Name = "Surname")]
            public string Surname { get; set; }

            /// <summary>
            /// Gets or sets the i d_code.
            /// </summary>
            [StringLength(11, MinimumLength = 0)]
            [Display(Name = "ID")]
            [EstonianId]
            public string? ID_code { get; set; }

        }

        /// <summary>
        /// Loads the async.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        private async Task LoadAsync(CustomerUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                Surname = user.Surname,
                ID_code = user.ID_code
            };
        }

        /// <summary>
        /// Ons the get async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        /// <summary>
        /// Ons the post async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (user.Surname != Input.Surname || user.FirstName != Input.FirstName || user.ID_code != Input.ID_code)
            {
                user.Surname = Input.Surname;
                user.FirstName = Input.FirstName;
                user.ID_code = Input.ID_code;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set your data.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
