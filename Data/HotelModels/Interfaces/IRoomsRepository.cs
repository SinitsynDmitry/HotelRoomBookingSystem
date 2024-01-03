/******************************************************************************
 *
 * File: IRoomsRepository.cs
 *
 * Description: IRoomsRepository.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelModels.Dtos;
using HotelModels.Select;

namespace HotelModels.Interfaces
{
    public interface IRoomsRepository
    {

        /// <summary>
        /// Gets the room async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns>A Task.</returns>
        Task<RoomAndBookings> GetRoomAsync(int id, DateRange dateRange);

        /// <summary>
        /// Gets the room async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        Task<Room> GetRoomAsync(int id);
        /// <summary>
        /// Gets the rooms async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Room>> GetRoomsAsync();

        /// <summary>
        /// Gets the available rooms async.
        /// </summary>
        /// <param name="roomsRequest">The rooms request.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<RoomAndBookings>> GetAvailableRoomsAsync(Rooms_SelectAndOrder roomsRequest);

    }
}