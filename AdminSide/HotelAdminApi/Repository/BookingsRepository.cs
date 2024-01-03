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
using HotelModels.Select;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HotelAdminApi.Repository
{
    public class BookingsRepository : IBookingsAdminRepository
    {
        private readonly Hotel_DbContext _context;

        public BookingsRepository(Hotel_DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the bookings async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Booking>> GetBookingsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return new List<Booking> { new Booking() { Id = -2 } }; // User not found
            }

            return await _context.Bookings.Where(b => b.Customer.Id == userId).Include(b => b.Room).Include(b => b.Customer).ToListAsync();
        }

        /// <summary>
        /// Gets the bookings async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Booking>> GetBookingsAsync()
        {
            return await _context.Bookings.Include(b => b.Room).Include(b => b.Customer).ToListAsync();
        }

        /// <summary>
        /// Gets the bookings async.
        /// </summary>
        /// <param name="order_by">The order_by.</param>
        /// <param name="roomId">The room id.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Booking>> GetBookingsAsync(string order_by = "", int roomId = -1)
        {
            switch (order_by)
            {
                case "start_asc":
                    return await _context.Bookings.Where(b => roomId < 0 || b.Room.Id == roomId).Include(b => b.Room).Include(b => b.Customer).OrderBy(s => s.StartTime).ToListAsync();
                    break;
                case "customer_asc":
                    return await _context.Bookings.Where(b => roomId < 0 || b.Room.Id == roomId).Include(b => b.Room).Include(b => b.Customer).OrderBy(s => s.Customer.FullName).ToListAsync();
                    break;
                case "customer_desc":
                    return await _context.Bookings.Where(b => roomId < 0 || b.Room.Id == roomId).Include(b => b.Room).Include(b => b.Customer).OrderByDescending(s => s.Customer.FullName).ToListAsync();
                    break;

                case "room_asc":
                    return await _context.Bookings.Where(b => roomId < 0 || b.Room.Id == roomId).Include(b => b.Room).Include(b => b.Customer).OrderBy(s => s.Room.Name).ToListAsync();
                    break;
                case "room_desc":
                    return await _context.Bookings.Where(b => roomId < 0 || b.Room.Id == roomId).Include(b => b.Room).Include(b => b.Customer).OrderByDescending(s => s.Room.Name).ToListAsync();
                    break;

                default:
                    return await _context.Bookings.Where(b => roomId < 0 || b.Room.Id == roomId).Include(b => b.Room).Include(b => b.Customer).OrderByDescending(s => s.StartTime).ToListAsync();
                    break;
            }
        }


        /// <summary>
        /// Gets the booking async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        public async Task<Booking> GetBookingAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

    }
}
