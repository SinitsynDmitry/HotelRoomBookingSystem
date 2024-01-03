/******************************************************************************
 *
 * File: IBookingsRepository.cs
 *
 * Description: IBookingsRepository.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/


namespace HotelModels.Interfaces
{
    public interface IBookingsRepository
    {

        /// <summary>
        /// Gets the user bookings async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId);

        /// <summary>
        /// Gets the user booking async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        Task<Booking> GetUserBookingAsync(int id, string userId = "");

        /// <summary>
        /// Creates the booking async.
        /// </summary>
        /// <param name="romId">The rom id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="description">The description.</param>
        /// <returns>A Task.</returns>
        Task<Booking> CreateBookingAsync(BookingLight booking);

        /// <summary>
        /// Deletes the room async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        Task<int> DeleteBookingAsync(int id, string userId);


    }
}