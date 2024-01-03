/******************************************************************************
 *
 * File: UserInfoResponse.cs
 *
 * Description: UserInfoResponse.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

namespace HotelModels
{
    public class UserInfoResponse
    {
        /// <summary>
        /// Gets or sets the customer user.
        /// </summary>
        public CustomerUser CustomerUser { get; set; }
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public IList<string> Roles { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether email is confirmed.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }
    }
}
