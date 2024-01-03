/******************************************************************************
 *
 * File: CustomerUser.cs
 *
 * Description: CustomerUser.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelModels
{
    public class CustomerUser : IdentityUser
    {
        public CustomerUser() : base() { }

        public CustomerUser(string userName) : base(userName) { }

        public CustomerUser(string email, string firstName, string surname, string iD_code, string phone) : base(email)
        {
            FirstName = firstName;
            Surname = surname;
            ID_code = iD_code is null ? "" : iD_code;
            PhoneNumber = phone;
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [PersonalData]
        [Display(Name = "First Name")]
        [MaxLength(100)]
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        [PersonalData]
        [Display(Name = "Surname")]
        [MaxLength(100)]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the i d_code.
        /// </summary>
        [PersonalData]
        [Display(Name = "ID")]
        [MaxLength(11)]
        public string ID_code { get; set; } = "";

        /// <summary>
        /// Gets the full name.
        /// </summary>
        [NotMapped]
        [Display(Name = "Full name")]
        public string FullName
        {
            get
            {
                return $"{FirstName} {Surname}";
            }
        }

        /// <summary>
        /// Tos the string.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return $"{FullName} | {Email} | {ID_code}";
        }
    }
}
