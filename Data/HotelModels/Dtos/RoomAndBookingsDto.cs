/******************************************************************************
 *
 * File: RoomAndBookingsDto.cs
 *
 * Description: RoomAndBookingsDto.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelModels.Dtos
{
    public record RoomAndBookingsDto( RoomDto Room, IEnumerable<BookingDto> Bookings);
}

