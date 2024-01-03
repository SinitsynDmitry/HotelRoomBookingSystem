/******************************************************************************
 *
 * File: Room.cs
 *
 * Description: Room.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using System.ComponentModel.DataAnnotations;

namespace HotelModels
{
    public record Room: BaseModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the number of beds.
        /// </summary>
        [Display(Name = "Beds")]
        [Range(1, 5)]
        public int NumberOfBeds { get; set;}

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        [Display(Name = "Price")]
        [Range(1, 500)]
        public float Price { get; set; }

        /// <summary>
        /// Tos the string.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return $"{Name} | Beds = {NumberOfBeds}";
        }
    }
}
