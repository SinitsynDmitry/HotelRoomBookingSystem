/******************************************************************************
 *
 * File: Base_SelectAndOrder.cs
 *
 * Description: Base_SelectAndOrder.cs class and he's methods.
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
    public record Base_SelectAndOrder
    {
        /// <summary>
        /// Gets the Date Range.
        /// </summary>
        public DateRange? DateRange { get; init; }

        /// <summary>
        /// Gets the order_by.
        /// </summary>
        public string Order_by { get; init; } = "price_asc";

        public Base_SelectAndOrder()
        {
            DateRange = new DateRange();
        }

    }

}
