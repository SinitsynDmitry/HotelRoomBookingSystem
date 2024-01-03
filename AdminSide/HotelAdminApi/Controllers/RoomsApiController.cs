/******************************************************************************
 *
 * File: RoomsApiController.cs
 *
 * Description: RoomsApiController.cs class and he's methods.
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
using Microsoft.EntityFrameworkCore;

namespace HotelAdminApi.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    // [ApiKey]
    public class RoomsApiController : ControllerBase
    {
        private readonly IRoomsAdminRepository _repository;

        public RoomsApiController(IRoomsAdminRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Rooms
        /// <summary>
        /// Gets the rooms async.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IEnumerable<RoomDto>> GetRoomsAsync()
        {
            return (await _repository.GetRoomsAsync()).Select(a=>a.AsDto());
        }

        // GET: api/Rooms/5
        /// <summary>
        /// Gets the room async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetRoomAsync(int id)
        {
            var room = await _repository.GetRoomAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return room.AsDto();
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


        // PUT: api/Rooms/5
        /// <summary>
        /// Updates the room async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="room">The room.</param>
        /// <returns>A Task.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomAsync(int id, RoomDto room)
        {
            if (id != room.Id)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateRoomAsync(room.AsOriginal());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RoomExistsAsync(id))
                {
                    return NotFound();
                }  
                    throw;
            }

            return NoContent();
        }

        // POST: api/Rooms
        /// <summary>
        /// Creates the room async.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        public async Task<ActionResult<RoomDto>> CreateRoomAsync(CreateRoomDto room)
        { 
            var newRoom = await _repository.CreateRoomAsync(room);

            return CreatedAtAction("GetRoom", new { id = newRoom.Id }, newRoom.AsDto());
        }

        // DELETE: api/Rooms/5
        /// <summary>
        /// Deletes the room async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomAsync(int id)
        {
            var result = await _repository.DeleteRoomAsync(id);
            if (result == -2)
            {
                return BadRequest("The room is booked");
            }
            if (result == -1)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Rooms the exists async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        private async Task<bool> RoomExistsAsync(int id)
        {
            return await _repository.RoomExistsAsync(id);
        }
    }
}
