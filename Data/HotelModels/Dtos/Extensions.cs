/******************************************************************************
 *
 * File: Extensions.cs
 *
 * Description: Extensions.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelModels.Dtos;
using System.Linq;

namespace HotelModels
{
    public static class Extensions
    {

        #region Room
        /// <summary>
        /// As the dto.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <returns>A RoomDto.</returns>
        public static RoomDto AsDto(this Room room)
        {
            if (room == null) return null;
            return new RoomDto()
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                NumberOfBeds = room.NumberOfBeds,
                Price = room.Price,
            };
        }

        /// <summary>
        /// As the original.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <returns>A Room.</returns>
        public static Room AsOriginal(this RoomDto room)
        {
            return new Room()
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                NumberOfBeds = room.NumberOfBeds,
                Price = room.Price
            };
        }

        /// <summary>
        /// As the original.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <returns>A Room.</returns>
        public static Room AsOriginal(this CreateRoomDto room)
        {
            return new Room()
            {
                Name = room.Name,
                Description = room.Description,
                NumberOfBeds = room.NumberOfBeds,
                Price = room.Price
            };
        }

        #endregion Room

        #region RoomAndBookings
        /// <summary>
        /// As the dto.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>A RoomAndBookingsDto.</returns>
        public static RoomAndBookingsDto AsDto(this RoomAndBookings item)
        {
            if (item == null) return null;
            var Room = item.Room.AsDto();
            var Bookings = item.Bookings.Select(i => i.BookingAsDto());

            return new RoomAndBookingsDto(Room, Bookings);
        }

        /// <summary>
        /// As the original.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>A RoomAndBookings.</returns>
        public static RoomAndBookings AsOriginal(this RoomAndBookingsDto dto)
        {
            if (dto == null) return null;
            var Room = dto.Room.AsOriginal();
            var Bookings = dto.Bookings.Select(i => i.BookingDtoAsOriginal());

            return new RoomAndBookings(Room, Bookings);
        }
        #endregion RoomAndBookings
        #region Booking
        /// <summary>
        /// Bookings the as dto.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns>A BookingDto.</returns>
        public static BookingDto BookingAsDto(this Booking booking)
        {
            if (booking == null) return null;
            return new BookingDto()
            {
                Id = booking.Id,
                Duration = booking.Duration,
                StartTime = booking.StartTime,
                Description = booking.Description,
                Room = booking.Room.AsDto(),
                Customer = booking.Customer.AsDto()
            };
        }

        /// <summary>
        /// Bookings the dto as original.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>A Booking.</returns>
        public static Booking BookingDtoAsOriginal(this BookingDto dto)
        {
            if (dto == null) return null;
            return new Booking()
            {
                Id = dto.Id,
                Duration = dto.Duration,
                StartTime = dto.StartTime,
                Description = dto.Description,
                Room = dto.Room.AsOriginal(),
                Customer = dto.Customer.AsOriginal()
            };
        }

        /// <summary>
        /// Creates the booking dto as original.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns>A Booking.</returns>
        public static Booking CreateBookingDtoAsOriginal(this CreateBookingDto booking)
        {
            if (booking == null) return null;
            return new Booking()
            {
                Duration = booking.Duration,
                StartTime = booking.StartTime,
                Description = booking.Description,
                Room = booking.Room.AsOriginal(),
                Customer = booking.Customer.AsOriginal()
            };
        }

        #endregion Booking

        #region CustomerUser
        /// <summary>
        /// As the dto.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A CustomerUserDto.</returns>
        public static CustomerUserDto AsDto(this CustomerUser user)
        {
            if (user == null) return null;
            return new CustomerUserDto()
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                Surname = user.Surname,
                ID_code = user.ID_code,
                PhoneNumber = user.PhoneNumber,
            };
        }

        /// <summary>
        /// As the original.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A CustomerUser.</returns>
        public static CustomerUser AsOriginal(this CustomerUserDto user)
        {
            if (user == null) return null;
            return new CustomerUser()
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                Surname = user.Surname,
                ID_code = user.ID_code,
                PhoneNumber = user.PhoneNumber,
            };
        }

        #endregion CustomerUser


    }
}
