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

using HotelApplication.ViewModels;
using HotelModels.Dtos;
using HotelModels.Interfaces;
using HotelModels.Select;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApplication.Controllers
{
    [Authorize]
    public class RoomsController : Controller
    {
        private readonly IRoomsRepository _service;

        public RoomsController(IRoomsRepository service)
        {
            _service = service;
        }

        // GET: Rooms
        /// <summary>
        /// Indices the.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="daterange">The daterange.</param>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> Index([FromQuery] Rooms_SelectAndOrder request, string daterange, string sortOrder)
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

            if (request.MaxBeds < request.MinBeds)
            {
                request = request with
                {
                    MaxBeds = request.MinBeds,
                    MinBeds = request.MaxBeds
                };
            }

            if (request.MaxPrice < request.MinPrice)
            {
                request = request with
                {
                    MaxPrice = request.MinPrice,
                    MinPrice = request.MaxPrice
                };
            }

            var rooms = await _service.GetAvailableRoomsAsync(request);
            var model = new SearchRoomsViewModel()
            {
                Rooms = rooms,
                RoomSearchRequest = request
            };


            return View(model);
        }

        // GET: Rooms/Details/5
        /// <summary>
        /// Details the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="daterange">The daterange.</param>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> Details(int? id, string daterange)
        {
            if (id == null)
            {
                return View(null);
            }
            var range = new DateRange();
            if (!string.IsNullOrWhiteSpace(daterange))
            {

                var dates = DateRange.ConvertDaterange(daterange);
                if (dates != null)
                {
                    if (dates.Item1 != null)
                    {
                        range = range with { StartTime = dates.Item1 };
                    }
                    if (dates.Item2 != null)
                    {
                        range = range with { EndTime = dates.Item2 };
                    }
                }
            }

            var roomPlus = await _service.GetRoomAsync(id.Value, range);
            if (roomPlus == null || roomPlus.Room == null || roomPlus.Room.Id == -1)
            {
                return View(null);
            }
            ViewData["DateRange"] = range.ToString();
            ViewData["Days"] = range.GetDaysBetween();

            return View(roomPlus);
        }
    }
}
