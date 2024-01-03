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

using HotelApplication.Helpers;
using HotelModels.Dtos;
using HotelModels.Helpers;
using HotelModels.Interfaces;
using HotelModels.Select;
using System.Net;

namespace HotelApplication.Service
{
    public class RoomsService: BaseApiService, IRoomsRepository
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

                var result = (await response.ReadContentAsync<List<RoomDto>>()).Select(r => r.AsOriginal());

                return result;
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
        /// Gets the available rooms async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<RoomAndBookings>> GetAvailableRoomsAsync(Rooms_SelectAndOrder request)
        {

            var path = $"api/rooms/available-rooms";
           
            string queryString = QueryStringBuilder.BuildQueryString(request);
            if(!string.IsNullOrWhiteSpace(queryString))
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
    }
}
