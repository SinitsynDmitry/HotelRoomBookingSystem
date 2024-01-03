/******************************************************************************
 *
 * File: IBookingsAdminRepository.cs
 *
 * Description: IBookingsAdminRepository.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelModels.Select;

namespace HotelModels.Interfaces
{
    public interface IBookingsAdminRepository
    {
        /// <summary>
        /// Gets the booking async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        Task<Booking> GetBookingAsync(int id);

        /// <summary>
        /// Gets the bookings async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Booking>> GetBookingsAsync();


        /// <summary>
        /// Gets the bookings async.
        /// </summary>
        /// <param name="order_by">The order_by.</param>
        /// <param name="roomId">The room id.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Booking>> GetBookingsAsync(string order_by = "", int roomId = -2);

        /// <summary>
        /// Gets the bookings async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Booking>> GetBookingsAsync(string userId);

    }
}
