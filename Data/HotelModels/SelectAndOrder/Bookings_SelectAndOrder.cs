/******************************************************************************
 *
 * File: Bookings_SelectAndOrder.cs
 *
 * Description: Bookings_SelectAndOrder.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelModels.Dtos;

namespace HotelModels.Select
{
    public record Bookings_SelectAndOrder : Base_SelectAndOrder
    {

        /// <summary>
        /// Gets the selected room id.
        /// </summary>
        public int SelectedRoomId { get; init; } = -1;

        public Bookings_SelectAndOrder()
        {
            
            DateRange = new DateRange();
            Order_by = "start_desc";
        }

    }

}
