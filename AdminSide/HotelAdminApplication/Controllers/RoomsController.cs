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

using HotelAdminApplication.ViewModels;
using HotelModels;
using HotelModels.Dtos;
using HotelModels.Helpers;
using HotelModels.Interfaces;
using HotelModels.Select;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelAdminApplication.Controllers
{
    [Authorize(Roles = $"{Constants.AdministratorsRole},{Constants.ManagersRole}")]
    public class RoomsController : Controller
    {
        private readonly IRoomsAdminRepository _service;

        public RoomsController(IRoomsAdminRepository service)
        {
            _service = service;
        }

        // GET: Rooms
        //[Authorize]
        /// <summary>
        /// Indices the.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="daterange">The daterange.</param>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> Index([FromQuery] Rooms_SelectAndOrder request, string daterange)
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

            IEnumerable<RoomAndBookings> rooms = new List<RoomAndBookings>();
            try
            {
                rooms = await _service.GetAvailableRoomsAsync(request);
            }
            catch (Exception ex)
            {

            }
            var model = new SearchRoomsViewModel()
            {
                Rooms = rooms,
                SelectAndOrder = request
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

        // GET: Rooms/Create
        /// <summary>
        /// Creates the.
        /// </summary>
        /// <returns>An IActionResult.</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,Name,NumberOfBeds,Price")] CreateRoomDto room)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var created = await _service.CreateRoomAsync(room);
                    if (created.Id > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {

                }
                ModelState.AddModelError("ModelOnly", "Room not found");
            }
            return View(room);
        }

        // GET: Rooms/Edit/5
        /// <summary>
        /// Edits the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View(null);
            }

            var room = await _service.GetRoomAsync(id.Value);
            if (room == null)
            {
                return View(null);
            }
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edits the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="room">The room.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Name,NumberOfBeds,Price")] Room room)
        {
            if (id != room.Id)
            {
                return View(null);
            }

            if (ModelState.IsValid)
            {
                var result = await _service.UpdateRoomAsync(room);
                if (result < 0)
                {
                    return View(null);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Rooms/Delete/5
        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                ViewData["Error"] = "Not Found";
                return View(null);
            }

            var room = await _service.GetRoomAsync(id.Value);
            if (room == null)
            {
                ViewData["Error"] = "Not Found";
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        /// <summary>
        /// Deletes the confirmed.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _service.GetRoomAsync(id);
            if (room != null)
            {
                try
                {
                    var result = await _service.DeleteRoomAsync(id);
                    if (result < 0)
                    {
                        ViewData["Error"] = "Not Found";
                        return View("Delete",room);
                    }
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = ex.Message;
                    return View("Delete", room);
                }
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
