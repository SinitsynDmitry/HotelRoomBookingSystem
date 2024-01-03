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

using HotelAdminApplication.ViewModels;
using HotelModels.Dtos;
using HotelModels.Helpers;
using HotelModels.Interfaces;
using HotelModels.Select;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelAdminApplication.Controllers
{
    [Authorize(Roles = $"{Constants.AdministratorsRole},{Constants.ManagersRole}")]
    public class BookingsController : Controller
    {
       
        private readonly IBookingsAdminRepository _bookings_service;
        private readonly IRoomsAdminRepository _rooms_service;
        private readonly TimeSpan _checkInTime;
        private readonly int _cancellationLimit;

        public BookingsController(IBookingsAdminRepository bookings_service, IRoomsAdminRepository rooms_service, IConfiguration configuration)
        {        
            _bookings_service = bookings_service;
            _rooms_service = rooms_service;
            _checkInTime = configuration.GetValue<TimeSpan>(Constants.CheckInTimeName);
            _cancellationLimit = configuration.GetValue<int>(Constants.CancellationLimitName);
        }


        // GET: Bookings
        /// <summary>
        /// Indices the.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="daterange">The daterange.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> Index([FromQuery] Bookings_SelectAndOrder request, string daterange, string sortOrder)
        {
            if (!string.IsNullOrWhiteSpace(daterange))
            {
                var dates = DateRange.ConvertDaterange(daterange);
                if (dates != null)
                {
                    if (dates.Item1 != null)
                    {
                        var date_Range = request.DateRange with { StartTime = dates.Item1 };
                        request = request with { DateRange = date_Range };
                    }
                    if (dates.Item2 != null)
                    {
                        var date_Range = request.DateRange with { EndTime = dates.Item2 };
                        request = request with { DateRange = date_Range };
                    }
                }
            }
            ViewData["CheckInTime"] = _checkInTime;
            ViewData["CancellationLimit"] = _cancellationLimit;

            var items = await _bookings_service.GetBookingsAsync(request.Order_by, request.SelectedRoomId);

            List<Tuple<int, string>> roomsList = new List<Tuple<int, string>>() { new Tuple<int, string>(-1, "All") };

            var rooms = await _rooms_service.GetRoomsAsync();
            if (rooms != null)
            {
                roomsList.AddRange(rooms.Select(r => new Tuple<int, string>(r.Id, r.Name)).OrderBy(t => t.Item2));
            }

            var model = new SearchBookinsViewModel()
            {
                Bookings = items,
                SelectAndOrder = request,
                RoomsList = roomsList
            };


            return View(model);
        }

        // GET: Bookings/Details/5
        /// <summary>
        /// Details the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> Details(int? id)
        {

            var booking = await _bookings_service.GetBookingAsync(id.Value);
            if (booking == null)
            {
                return RedirectToAction(nameof(Index));
            }
            booking.SetCancellationDeadline(_checkInTime, _cancellationLimit);

            return View(booking);
        }
    }
}
