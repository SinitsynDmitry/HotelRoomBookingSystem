/******************************************************************************
 *
 * File: RoomsController.cs
 *
 * Description: RoomsController.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelModels;
using HotelModels.Dtos;
using HotelModels.Interfaces;
using HotelModels.Select;
using Microsoft.AspNetCore.Mvc;

namespace HotelCustomerApi.Controllers
{
    [Route("api/rooms")]
    [ApiController]
   // [ApiKey]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomsRepository _repository;

        public RoomsController(IRoomsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Rooms
        /// <summary>
        /// Gets the rooms async.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetRoomsAsync()
        {
            return Ok((await _repository.GetRoomsAsync()).Select(a => a.AsDto()));
        }

        /// <summary>
        /// Gets the available rooms async.
        /// </summary>
        /// <param name="roomsRequest">The rooms request.</param>
        /// <returns>A Task.</returns>
        [HttpGet("available-rooms")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAvailableRoomsAsync([FromQuery] Rooms_SelectAndOrder roomsRequest)
        {
            if (roomsRequest.DateRange.StartTime > roomsRequest.DateRange.EndTime)
            {
                return BadRequest("Invalid time range");
            }

            try
            {
                var availableRooms = (await _repository.GetAvailableRoomsAsync(roomsRequest)).Select(a => a.AsDto());

                return Ok(availableRooms);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/RoomAndBookings/{id}
        /// <summary>
        /// Gets the RoomAndBookings async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet("room-and-bookings/{id}")]
        public async Task<ActionResult<RoomAndBookingsDto>> GetRoomAndBookingsAsync(int id, [FromQuery] DateRange dateRange)
        {
            var roomPlus = await _repository.GetRoomAsync(id, dateRange);

            if (roomPlus == null || roomPlus.Room == null || roomPlus.Room.Id == -1)
            {
                return NotFound();
            }

            return Ok(roomPlus.AsDto());
        }

    }
}
