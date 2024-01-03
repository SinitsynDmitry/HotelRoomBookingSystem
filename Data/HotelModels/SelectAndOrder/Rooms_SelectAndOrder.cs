/******************************************************************************
 *
 * File: Rooms_SelectAndOrder.cs
 *
 * Description: Rooms_SelectAndOrder.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelModels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelModels.Select
{
    public record Rooms_SelectAndOrder: Base_SelectAndOrder
    {

        /// <summary>
        /// Gets the min beds.
        /// </summary>
        public int? MinBeds { get; init; } = 1;
        /// <summary>
        /// Gets the max beds.
        /// </summary>
        public int? MaxBeds { get; init; } = 5;

        /// <summary>
        /// Gets the min price.
        /// </summary>
        public float? MinPrice { get; init; } = 0;
        /// <summary>
        /// Gets the max price.
        /// </summary>
        public float? MaxPrice { get; init; } = 500;

        public Rooms_SelectAndOrder():base()
        {
            DateRange = new DateRange();
            Order_by = "price_asc";
        }

    }

}
