/******************************************************************************
 *
 * File: CustomerUserDto.cs
 *
 * Description: CustomerUserDto.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/


using System.ComponentModel.DataAnnotations;

namespace HotelModels
{
    public class CustomerUserDto 
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public  string Id { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public  string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [MaxLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        [MaxLength(100)]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the i d_code.
        /// </summary>
        [MaxLength(11)]
        public string? ID_code { get; set; } = "";

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public string? PhoneNumber { get; set; }
    }
}
