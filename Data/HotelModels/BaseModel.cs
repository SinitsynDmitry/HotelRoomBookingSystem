/******************************************************************************
 *
 * File: BaseModel.cs
 *
 * Description: BaseModel.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using System.ComponentModel.DataAnnotations;

namespace HotelModels
{
    public record BaseModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Display(Name = "Note")]
        public string Description { get; set; } = "";
    }
}
