/******************************************************************************
 *
 * File: ErrorViewModel.cs
 *
 * Description: ErrorViewModel.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

namespace HotelApplication.Models
{
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether show request id.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
