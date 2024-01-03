/******************************************************************************
 *
 * File: SearchRoomsViewModel.cs
 *
 * Description: SearchRoomsViewModel.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelModels.Select;

namespace HotelApplication.ViewModels
{
    public class SearchRoomsViewModel
    {
        /// <summary>
        /// Gets or sets the room search request.
        /// </summary>
        public Rooms_SelectAndOrder RoomSearchRequest { get; set; }
        /// <summary>
        /// Gets or sets the rooms.
        /// </summary>
        public IEnumerable<RoomAndBookings> Rooms { get; set; }
    }
}
