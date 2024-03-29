﻿/******************************************************************************
 *
 * File: Hotel_DbContext.cs
 *
 * Description: Hotel_DbContext.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;

namespace HotelData
{
    public class Hotel_DbContext : IdentityDbContext<CustomerUser>
    {
        public Hotel_DbContext( DbContextOptions<Hotel_DbContext> options)
            : base(options)
        {

        }

        /// <summary>
        /// Gets or sets the rooms.
        /// </summary>
        public DbSet<Room> Rooms { get; set; }

        /// <summary>
        /// Gets or sets the bookings.
        /// </summary>
        public DbSet<Booking> Bookings { get; set; }

    }
}
