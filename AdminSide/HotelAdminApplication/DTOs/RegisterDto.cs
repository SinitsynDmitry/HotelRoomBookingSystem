/******************************************************************************
 *
 * File: RegisterDto.cs
 *
 * Description: RegisterDto.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using System.ComponentModel.DataAnnotations;
using HotelAdminApplication.Validations;

namespace HotelAdminApplication.DTOs
{
    public class RegisterDto
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
        /// Gets or sets the id_code.
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

        [Display(Name = "Role")]
        public string Role { get; set; }

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

}
