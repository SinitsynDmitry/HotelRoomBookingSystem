/******************************************************************************
 *
 * File: RoomsService.cs
 *
 * Description: RoomsService.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelAdminApplication.Helpers;
using HotelModels;
using HotelModels.Dtos;
using HotelModels.Helpers;
using HotelModels.Interfaces;
using HotelModels.Select;
using System.Net;

namespace HotelAdminApplication.Service
{
    public class RoomsService : BaseApiService, IRoomsAdminRepository
    {
        public RoomsService(IHttpClientFactory httpClientFactory, string apiKey, string basePath) : base(httpClientFactory, apiKey, basePath)
        {

        }

        /// <summary>
        /// Gets the rooms async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Room>> GetRoomsAsync()
        {
            var path = "api/rooms";

            using (HttpClient client = CreateClient())
            {
                var response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    var result = (await response.ReadContentAsync<List<RoomDto>>()).Select(r => r.AsOriginal());

                    return result;
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                string errorContent = await response.Content.ReadAsStringAsync();

                throw new Exception(errorContent);
            }
        }

        /// <summary>
        /// Gets the available rooms async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<RoomAndBookings>> GetAvailableRoomsAsync(Rooms_SelectAndOrder request)
        {
            var path = $"api/rooms/available-rooms";

            string queryString = QueryStringBuilder.BuildQueryString(request);
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                path += $"?{queryString}";
            }

            using (HttpClient client = CreateClient())
            {
                var response = await client.GetAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    var result = (await response.ReadContentAsync<List<RoomAndBookingsDto>>()).Select(r => r.AsOriginal());

                    return result;
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                string errorContent = await response.Content.ReadAsStringAsync();

                throw new Exception(errorContent);
            }
        }

        /// <summary>
        /// Gets the room async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<RoomAndBookings> GetRoomAsync(int id, DateRange dateRange)
        {
            var path = $"api/rooms/room-and-bookings/{id}";
            using (HttpClient client = CreateClient())
            {
                string queryString = QueryStringBuilder.BuildQueryString(dateRange);
                if (!string.IsNullOrWhiteSpace(queryString))
                {
                    path += $"?{queryString}";
                }
                var response = await client.GetAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    var result = (await response.ReadContentAsync<RoomAndBookingsDto>())?.AsOriginal();

                    return result;
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                string errorContent = await response.Content.ReadAsStringAsync();

                throw new Exception(errorContent);
            }
        }

        /// <summary>
        /// Gets the room async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<Room> GetRoomAsync(int id)
        {
            var path = $"api/rooms/{id}";
            using (HttpClient client = CreateClient())
            {
                var response = await client.GetAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    var result = (await response.ReadContentAsync<RoomDto>())?.AsOriginal();

                    return result;
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                string errorContent = await response.Content.ReadAsStringAsync();

                throw new Exception(errorContent);
            }
        }


        /// <summary>
        /// Updates the room async.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <returns>A Task.</returns>
        public async Task<int> UpdateRoomAsync(Room room)
        {
            var path = $"api/rooms/{room.Id}";
            using (HttpClient client = CreateClient())
            {
                var response = await API_RequestAsync(client, path, room, Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod.Put);

                if (response.IsSuccessStatusCode)
                {
                    return 1;
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return -1;
                }

                string errorContent = await response.Content.ReadAsStringAsync();

                throw new Exception(errorContent);
            }
        }

        /// <summary>
        /// Creates the room async.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <returns>A Task.</returns>
        public async Task<Room> CreateRoomAsync(CreateRoomDto room)
        {
            var path = "api/rooms/";

            using (HttpClient client = CreateClient())
            {
                try
                {
                    var response = await API_RequestAsync(client, path, room);
                    response.EnsureSuccessStatusCode();
                    var result = (await response.ReadContentAsync<RoomDto>())?.AsOriginal();
                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes the room async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<int> DeleteRoomAsync(int id)
        {
            var path = $"api/rooms/{id}";
            using (HttpClient client = CreateClient())
            {
                var response = await client.DeleteAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    return 1;
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return -1;
                }

                string errorContent = await response.Content.ReadAsStringAsync();

                throw new Exception(errorContent);
            }
        }

        /// <summary>
        /// Rooms the exists async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<bool> RoomExistsAsync(int id)
        {
            var room = await GetRoomAsync(id);
            return room != null && room.Id == id;
        }

    }
}
