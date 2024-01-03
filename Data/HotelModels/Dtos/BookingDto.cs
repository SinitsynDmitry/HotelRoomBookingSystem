/******************************************************************************
 *
 * File: BookingDto.cs
 *
 * Description: BookingDto.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

namespace HotelModels
{
    public record BookingDto
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        public CustomerUserDto Customer { get; set; }

        /// <summary>
        /// Gets or sets the room.
        /// </summary>
        public RoomDto Room { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        public DateTimeOffset StartTime { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public int Duration {  get; set; }
    }
}
