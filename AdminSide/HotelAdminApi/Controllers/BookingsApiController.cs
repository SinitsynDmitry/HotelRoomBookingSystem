/******************************************************************************
 *
 * File: BookingsApiController.cs
 *
 * Description: BookingsApiController.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelModels;
using HotelModels.Authorization;
using HotelModels.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HotelAdminApi.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    [ApiKey]
    public class BookingsApiController : ControllerBase
    {
        private readonly IBookingsAdminRepository _repository;

        public BookingsApiController(IBookingsAdminRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Bookings
        /// <summary>
        /// Gets the bookings.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings()
        {
            return Ok((await _repository.GetBookingsAsync()).Select(a => a.BookingAsDto()));
        }
        /// <summary>
        /// Gets the user booking async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetUserBookingAsync([Required] int id)
        {
            var booking = await _repository.GetBookingAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking.BookingAsDto());
        }

        /// <summary>
        /// Gets the sorted bookings.
        /// </summary>
        /// <param name="order_by">The order_by.</param>
        /// <param name="roomId">The room id.</param>
        /// <returns>A Task.</returns>
        [HttpGet("sorted-bookings")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetSortedBookings(string order_by, int roomId)
        {
            return Ok((await _repository.GetBookingsAsync(order_by,roomId)).Select(a => a.BookingAsDto()));
        }
        /// <summary>
        /// Gets the user bookings async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        [HttpGet("user-bookings/{userId}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetUserBookingsAsync([Required] string userId)
        {
            var result = await _repository.GetBookingsAsync(userId);
            if (result?.Any(b => b.Id < 0) == true)
            {
                return BadRequest("User not found");
            }

            return Ok(result?.Select(a => a.BookingAsDto()));
        }

    }
}
