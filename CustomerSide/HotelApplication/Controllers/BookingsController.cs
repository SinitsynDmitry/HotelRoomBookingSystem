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

using HotelApplication.Service;
using HotelModels;
using HotelModels.Helpers;
using HotelModels.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelApplication.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly IBookingsRepository _service;
        private readonly TimeSpan _checkInTime;
        private readonly int _cancellationLimit;

        public BookingsController(IBookingsRepository service, IConfiguration configuration)
        {
            _service = service;
            _checkInTime =  configuration.GetValue<TimeSpan>(Constants.CheckInTimeName);
            _cancellationLimit = configuration.GetValue<int>(Constants.CancellationLimitName);
        }


        // GET: Bookings
        /// <summary>
        /// Indices the.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            ViewData["CheckInTime"] = _checkInTime;
            ViewData["CancellationLimit"] = _cancellationLimit;
            string userId = userIdClaim.Value;
            var result = await _service.GetUserBookingsAsync(userId);
           

            return View(result);
        }

        // GET: Bookings/Details/5
        /// <summary>
        /// Details the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            string userId = userIdClaim.Value;


            if (id == null)
            {
                return  RedirectToAction(nameof(Index));
            }

            var booking = await _service.GetUserBookingAsync(id.Value, userId);
            if (booking == null)
            {
                return RedirectToAction(nameof(Index));
            }
            booking.SetCancellationDeadline(_checkInTime, _cancellationLimit);

            return View(booking);
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId", "UserId", "StartTime,Duration,Description")] BookingLight booking, int RoomId = 0)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), "Rooms");
            }

            string userId = userIdClaim.Value;
            booking = booking with { UserId = userId };

            try
            {
                var created = await _service.CreateBookingAsync(booking);
                if (created.Id < 0)
                {
                    ModelState.AddModelError("ModelOnly", "Room not found");
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), "Rooms");
            }

            return RedirectToAction(nameof(Details));
        }

        // GET: Bookings/Delete/5
        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                ViewData["Error"] = "Not booking found.";
                return View();
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

           if (userIdClaim == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            string userId = userIdClaim.Value;

            var booking = await _service.GetUserBookingAsync(id.Value, userId);
            if (booking == null)
            {
                ViewData["Error"] = "Not booking found.";
                return View();
            }
            booking.SetCancellationDeadline(_checkInTime, _cancellationLimit);

            if (booking.IsIrrevocable)
            {
                ViewData["Error"] = $"The cancellation deadline is {booking.Deadline.ToString("dd.MM.yyyy HH:mm")}. You cannot cancel this booking.";
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        /// <summary>
        /// Deletes the confirmed.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            string userId = userIdClaim.Value;

            var booking = await _service.GetUserBookingAsync(id, userId);
            if (booking == null)
            {
                ViewData["Error"] = "Not booking found.";
                return View("Delete", booking);
            }

            booking.SetCancellationDeadline(_checkInTime, _cancellationLimit);

            if (booking.IsIrrevocable)
            {
                ViewData["Error"] = $"The cancellation deadline is {booking.Deadline.ToString("dd.MM.yyyy HH:mm")}. You cannot cancel this booking.";
                return View("Delete",booking);
            }

            try
            {
                var x = await _service.DeleteBookingAsync(id, userId);
                if (x < 0)
                {
                    ViewData["Error"] = "Not booking found.";
                    return View("Delete", booking);
                }
            }
            catch (Exception ex)
            {
                ViewBag["Error"] = ex.Message;
                return View("Delete", booking);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
