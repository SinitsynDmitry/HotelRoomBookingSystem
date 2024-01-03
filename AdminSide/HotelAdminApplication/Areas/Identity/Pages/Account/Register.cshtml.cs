﻿/******************************************************************************
 *
 * File: Register.cshtml.cs
 *
 * Description: Register.cshtml.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

#nullable disable

using HotelAdminApplication.Validations;
using HotelModels;
using HotelModels.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace HotelAdminApplication.Areas.Identity.Pages.Account
{
    [Authorize(Roles = Constants.AdministratorsRole)]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<CustomerUser> _signInManager;
        private readonly UserManager<CustomerUser> _userManager;
        private readonly IUserStore<CustomerUser> _userStore;
        private readonly IUserEmailStore<CustomerUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<CustomerUser> userManager,
            IUserStore<CustomerUser> userStore,
            SignInManager<CustomerUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }


        /// <summary>
        /// Gets or sets the return url.
        /// </summary>
        public string ReturnUrl { get; set; }

        public class InputModel
        {
            /// <summary>
            /// Gets or sets the email.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

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

            /// <summary>
            /// Gets or sets the phone number.
            /// </summary>
            [Display(Name = "Phone")]
            [Phone]
            public string? PhoneNumber { get; set; }

            /// <summary>
            /// Gets or sets the password.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            /// Gets or sets the confirm password.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        /// <summary>
        /// Ons the get async.
        /// </summary>
        /// <param name="returnUrl">The return url.</param>
        /// <returns>A Task.</returns>
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
           // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        /// <summary>
        /// Ons the post async.
        /// </summary>
        /// <param name="returnUrl">The return url.</param>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
           // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new CustomerUser(Input.Email, Input.FirstName, Input.Surname, Input.ID_code, Input.PhoneNumber);
               

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (_userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    }

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <returns>A CustomerUser.</returns>
        private CustomerUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<CustomerUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(CustomerUser)}'. " +
                    $"Ensure that '{nameof(CustomerUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
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
    }
}
