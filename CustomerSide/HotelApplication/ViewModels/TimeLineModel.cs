/******************************************************************************
 *
 * File: TimeLineModel.cs
 *
 * Description: TimeLineModel.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

namespace HotelApplication.ViewModels
{
    public record TimeLineModel(List<DateTimeOffset> DaysList, IEnumerable<Booking> Bookings, bool IsActive);

}
