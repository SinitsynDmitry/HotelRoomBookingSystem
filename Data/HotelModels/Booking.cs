/******************************************************************************
 *
 * File: Booking.cs
 *
 * Description: Booking.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace HotelModels
{
    public record Booking : BaseModel
    {
        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        public CustomerUser Customer { get; set; }

        /// <summary>
        /// Gets or sets the room.
        /// </summary>
        public Room Room { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        [Display(Name = "Date of entry")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTimeOffset StartTime { get; set; }


        /// <summary>
        /// Gets the deadline.
        /// </summary>
        [NotMapped]
        [Display(Name = "Cancellation deadline")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTimeOffset Deadline { get; private set; }


        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        [Display(Name = "Duration(days)")]
        public int Duration { get; set; }

        /// <summary>
        /// Gets a value indicating whether is irrevocable.
        /// </summary>
        [NotMapped]
        public bool IsIrrevocable { get; private set; }


        /// <summary>
        /// Sets the cancellation deadline.
        /// </summary>
        /// <param name="_checkInTime">The _check in time.</param>
        /// <param name="_cancellationLimit">The _cancellation limit.</param>
        public void SetCancellationDeadline(TimeSpan _checkInTime, int _cancellationLimit)
        {
            Deadline = (StartTime.Date + _checkInTime).AddDays(-1*_cancellationLimit);
            IsIrrevocable = Deadline <= DateTimeOffset.UtcNow;
        }
    }

    public record BookingLight([Required] int RoomId, [Required] string UserId, [Required] DateTimeOffset StartTime, [Range(1, int.MaxValue)] int Duration, string Description = "");
}
