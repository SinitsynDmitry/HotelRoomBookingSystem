/******************************************************************************
 *
 * File: BookingsRepository.cs
 *
 * Description: BookingsRepository.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelData;
using HotelModels;
using HotelModels.Helpers;
using HotelModels.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace HotelCustomerApi.Repository
{
    public class BookingsRepository : IBookingsRepository
    {
        private readonly Hotel_DbContext _context;
        private readonly TimeSpan _checkInTime;
        private readonly int _cancellationLimit;

        public BookingsRepository(Hotel_DbContext context, IConfiguration configuration)
        {
            _context = context;
            _checkInTime = configuration.GetValue<TimeSpan>(Constants.CheckInTimeName);
            _cancellationLimit = configuration.GetValue<int>(Constants.CancellationLimitName);
        }

        /// <summary>
        /// Gets the bookings async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId = "")
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return await _context.Bookings.Include(b => b.Room).Include(b => b.Customer).ToListAsync();
            }
            else
            {
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    return new List<Booking> { new Booking() { Id = -2 } }; // User not found
                }

                return await _context.Bookings.Where(b => b.Customer.Id == userId).Include(b => b.Room).Include(b => b.Customer).ToListAsync();
            }

        }


        /// <summary>
        /// Gets the booking async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        public async Task<Booking> GetUserBookingAsync(int id, string userId = "")
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return await _context.Bookings
                    .Include(b => b.Room)
                    .Include(b => b.Customer)
                    .FirstOrDefaultAsync(b => b.Id == id);
            }
            else
            {
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    return new Booking() { Id = -2 }; // User not found
                }

                return await _context.Bookings
                    .Where(b => b.Customer.Id == userId)
                    .Include(b => b.Room)
                    .Include(b => b.Customer)
                    .FirstOrDefaultAsync(b => b.Id == id);
            }
        }


        /// <summary>
        /// Creates the booking async.
        /// </summary>
        /// <param name="roomId">The room id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="duration">The duration.</param>
        /// <returns>A Task.</returns>
        public async Task<Booking> CreateBookingAsync(BookingLight bookingLight)
        {
            var room = await _context.Rooms.FindAsync(bookingLight.RoomId);
            var user = await _context.Users.FindAsync(bookingLight.UserId);

            if (room == null)
            {
                return new Booking() { Id = -1 }; // Room  not found
            }
            if (user == null)
            {
                return new Booking() { Id = -2 }; // User not found
            }
            var startNew = bookingLight.StartTime.Date;
            var endNew = startNew.AddDays(bookingLight.Duration);

            var prevBookings = await _context.Bookings.Where(b => b.Room.Id == bookingLight.RoomId && b.StartTime.Date.AddDays(b.Duration) > startNew && b.StartTime.Date < endNew).ToListAsync();

            if (prevBookings.Count > 0)
            {
                return new Booking() { Id = -3 }; // Booking conflict
            }

            // Create a new Booking instance
            var newBooking = new Booking
            {
                Room = room,
                Customer = user,
                StartTime = bookingLight.StartTime,
                Duration = bookingLight.Duration,
                Description = bookingLight.Description,
            };

            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();

            return newBooking;
        }


        /// <summary>
        /// Deletes the booking async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        public async Task<int> DeleteBookingAsync(int id, string userId = "")
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return -1;
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                return -2;
            }
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return -2; // User not found
            }
            booking = await _context.Bookings.Where(b => b.Customer.Id == userId && b.Id == id).FirstOrDefaultAsync();
            if (booking == null)
            {
                return -3;//Not your
            }

            booking.SetCancellationDeadline(_checkInTime, _cancellationLimit);

            if (booking.IsIrrevocable)
            {
                return -4;
            }

            _context.Bookings.Remove(booking);
            return await _context.SaveChangesAsync();

        }
    }
}
