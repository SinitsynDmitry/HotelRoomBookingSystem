/******************************************************************************
 *
 * File: BookingsController.cs
 *
 * Description: BookingsController.cs class and he's methods.
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

namespace HotelCustomerApi.Controllers
{

    [Route("api/bookings")]
    [ApiController]
    [ApiKey]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingsRepository _repository;

        public BookingsController(IBookingsRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the user bookings async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        [HttpGet("user-bookings/{userId}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetUserBookingsAsync([Required]string userId)
        {
            var result = await _repository.GetUserBookingsAsync(userId);
            if (result?.Any(b => b.Id < 0) == true)
            {
                return BadRequest("User not found");
            }

            return Ok(result?.Select(a => a.BookingAsDto()));
        }

        /// <summary>
        /// Gets the user booking async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet("user-bookings/{id}/{userId}")]
        public async Task<ActionResult<BookingDto>> GetUserBookingAsync([Required] int id, [Required] string userId)
        {
            var booking = await _repository.GetUserBookingAsync(id, userId);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking.BookingAsDto());
        }


        // POST: api/Bookings
        /// <summary>
        /// Creates the booking async.
        /// </summary>
        /// <param name="bookingLight">The booking light.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        public async Task<ActionResult<BookingDto>> CreateBookingAsync([FromBody] BookingLight bookingLight)
        {
            var newBooking = await _repository.CreateBookingAsync(bookingLight);
            if (newBooking?.Id < 0)
            {
                if (newBooking?.Id == -3)
                {
                    return BadRequest("Booking conflict");
                }

                if (newBooking?.Id < -1)
                {
                    return BadRequest("User not found");
                }
                return BadRequest("Room not found");
            }
            
           
            return CreatedAtAction("GetUserBooking", new { id = newBooking.Id, userId= bookingLight.UserId }, newBooking.BookingAsDto());
        }

        // DELETE: api/Bookings/5
        /// <summary>
        /// Deletes the user booking async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        [HttpDelete("user-bookings/{id}/{userId}")]
        public async Task<IActionResult> DeleteUserBookingAsync([Required][Range(1, int.MaxValue)] int id, [Required] string userId)
        {
            var result = await _repository.DeleteBookingAsync(id, userId);
            if (result == -3)
            {
                return BadRequest("Not allowed");
            }
            if (result == -2)
            {
                return BadRequest("Wrong user");
            }
            if (result < 0)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
