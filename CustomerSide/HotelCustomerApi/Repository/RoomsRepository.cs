/******************************************************************************
 *
 * File: RoomsRepository.cs
 *
 * Description: RoomsRepository.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelData;
using HotelModels;
using HotelModels.Dtos;
using HotelModels.Interfaces;
using HotelModels.Select;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelCustomerApi.Repository
{
    public class RoomsRepository : IRoomsRepository
    {
        private readonly Hotel_DbContext _context;

        public RoomsRepository(Hotel_DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the rooms async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Room>> GetRoomsAsync()
        {
            return await _context.Rooms.ToListAsync();
        }

        /// <summary>
        /// Gets the room async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<Room> GetRoomAsync(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        /// <summary>
        /// Gets the room async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns>A Task.</returns>
        public async Task<RoomAndBookings> GetRoomAsync(int id, DateRange dateRange)
        {
            var endTime = dateRange.EndTime.HasValue ? dateRange.EndTime : DateTimeOffset.MaxValue;
            var startTime = dateRange.StartTime.HasValue ? dateRange.StartTime : DateTimeOffset.MinValue;

            var bookings = await _context.Bookings
             .Where(b => (b.Room.Id == id && b.StartTime < endTime && b.StartTime.AddDays(b.Duration) > startTime))
             .ToListAsync();
            var room = await _context.Rooms.FindAsync(id);

            var result = new RoomAndBookings(room, bookings);

            return result;
        }

        /// <summary>
        /// Gets the available rooms async.
        /// </summary>
        /// <param name="roomsRequest">The rooms request.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<RoomAndBookings>> GetAvailableRoomsAsync(Rooms_SelectAndOrder roomsRequest)
        {
            var endTime = roomsRequest.DateRange.EndTime.HasValue ? roomsRequest.DateRange.EndTime : DateTimeOffset.MaxValue;
            var startTime = roomsRequest.DateRange.StartTime.HasValue ? roomsRequest.DateRange.StartTime : DateTimeOffset.MinValue;
            var minBeds = roomsRequest.MinBeds.HasValue ? roomsRequest.MinBeds : 0;
            var maxBeds = roomsRequest.MaxBeds.HasValue ? roomsRequest.MaxBeds : 5;
            var minPrice = roomsRequest.MinPrice.HasValue ? roomsRequest.MinPrice : 0;
            var maxPrice = roomsRequest.MaxPrice.HasValue ? roomsRequest.MaxPrice : float.MaxValue;

            var periodDays = (endTime - startTime).Value.TotalDays;

            var bookedRoomsSummary = await _context.Bookings
              .Where(b => (b.StartTime < endTime && b.StartTime.AddDays(b.Duration) > startTime))
              .GroupBy(b => b.Room.Id)
              .ToListAsync();

            var bookedRoomIds = bookedRoomsSummary.AsParallel()
                 .Select(group => new
                 {
                     RoomId = group.Key,
                     TotalCustomDays = group.Sum(b =>
                         b.StartTime < startTime
                             ? (startTime - b.StartTime).Value.Days
                             : b.Duration
                     )
                 })
                 .Where(summary => summary.TotalCustomDays >= periodDays)
                 .Select(item => item.RoomId).ToList();

            var availableRoomsPlus = await _context.Rooms
                .Where(room => !bookedRoomIds.Contains(room.Id) && room.NumberOfBeds >= minBeds && room.NumberOfBeds < maxBeds && room.Price >= minPrice && room.Price <= maxPrice)
                .OrderBy(r => r.Price)
                .Select(room => new RoomAndBookings(
                    room,
                    _context.Bookings
                    .Where(b => (b.Room.Id == room.Id && b.StartTime < endTime && b.StartTime.AddDays(b.Duration) > startTime))
                    .ToList()))
                .ToListAsync();



            return availableRoomsPlus;
        }

    }
}
