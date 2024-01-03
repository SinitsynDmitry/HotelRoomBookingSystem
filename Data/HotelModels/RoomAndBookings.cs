/******************************************************************************
 *
 * File: RoomAndBookings.cs
 *
 * Description: RoomAndBookings.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

namespace HotelModels
{
    public record RoomAndBookings(Room Room, IEnumerable<Booking> Bookings);
}
