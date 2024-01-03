/******************************************************************************
 *
 * File: SearchBookinsViewModel.cs
 *
 * Description: SearchBookinsViewModel.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelModels;
using HotelModels.Select;

namespace HotelAdminApplication.ViewModels
{
    public class SearchBookinsViewModel
    {
        /// <summary>
        /// Gets or sets the rooms list.
        /// </summary>
        public List<Tuple<int, string>> RoomsList { get; set; } = new List<Tuple<int, string>>() { new Tuple<int, string>(-1, "All") };

        /// <summary>
        /// Gets or sets the room search request.
        /// </summary>
        public Bookings_SelectAndOrder SelectAndOrder { get; set; }
        /// <summary>
        /// Gets or sets the rooms.
        /// </summary>
        public IEnumerable<Booking> Bookings { get; set; }
    }
}
