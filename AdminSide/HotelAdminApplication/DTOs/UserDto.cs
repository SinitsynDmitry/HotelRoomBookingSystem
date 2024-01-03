/******************************************************************************
 *
 * File: UserDto.cs
 *
 * Description: UserDto.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelModels;
using System.ComponentModel.DataAnnotations;

namespace HotelAdminApplication.DTOs
{
    public class UserDto: CustomerUser
    {
        [Display(Name = "Role")]
        public string Role {  get; set; }
    }
}
