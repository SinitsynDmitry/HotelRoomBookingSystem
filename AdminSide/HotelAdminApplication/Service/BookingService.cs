/******************************************************************************
 *
 * File: BookingService.cs
 *
 * Description: BookingService.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelAdminApplication.Helpers;
using HotelModels;
using HotelModels.Interfaces;
using System.Net;

namespace HotelAdminApplication.Service
{
    public class BookingService : BaseApiService, IBookingsAdminRepository
    {
        public BookingService(IHttpClientFactory httpClientFactory, string apiKey, string basePath) : base(httpClientFactory, apiKey, basePath)
        {
        }

        /// <summary>
        /// Gets the booking async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<Booking> GetBookingAsync(int id)
        {
            var path = $"api/bookings/{id}";

            using (HttpClient client = CreateClient())
            {
                var response = await client.GetAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    var result = (await response.ReadContentAsync<BookingDto>())?.BookingDtoAsOriginal();

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
        /// Gets the bookings async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Booking>> GetBookingsAsync()
        {
            var path = $"api/bookings";

            return await GetBookingsInternalAsync(path);
        }

        /// <summary>
        /// Gets the bookings sortable async.
        /// </summary>
        /// <param name="order_by">The order_by.</param>
        /// <param name="roomId">The room id.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Booking>> GetBookingsAsync(string order_by = "", int roomId = -1)
        {
            var path = $"api/bookings/sorted-bookings?order_by={order_by}&roomId={roomId}";

            return await GetBookingsInternalAsync(path);
        }

        /// <summary>
        /// Gets the user booking async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Booking>> GetBookingsAsync(string userId)
        {
           
            if (string.IsNullOrWhiteSpace(userId))
            {
              throw new ArgumentNullException(nameof(userId));
            }
            var path = $"api/bookings/user-bookings/{userId}";

            return await GetBookingsInternalAsync(path);
        }

        /// <summary>
        /// Gets the bookings internal async.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A Task.</returns>
        private async Task<IEnumerable<Booking>> GetBookingsInternalAsync(string path)
        {
            try
            {
                using (HttpClient client = CreateClient())
                {
                    var response = await client.GetAsync(path);
                    response.EnsureSuccessStatusCode();

                    var result = (await response.ReadContentAsync<List<BookingDto>>()).Select(r => r.BookingDtoAsOriginal());

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
