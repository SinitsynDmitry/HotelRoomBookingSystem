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

using HotelApplication.Helpers;
using HotelModels.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace HotelApplication.Service
{
    public class BookingService : BaseApiService, IBookingsRepository
    {
        public BookingService(IHttpClientFactory httpClientFactory, string apiKey, string basePath) : base(httpClientFactory, apiKey, basePath)
        {
        }


        /// <summary>
        /// Gets the user booking async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        public async Task<Booking> GetUserBookingAsync(int id, string userId)
        {
            var path = $"api/bookings/user-bookings/{id}/{userId}";
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
        /// Gets the user bookings async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Booking>> GetUserBookingsAsync([Required] string userId)
        {
            var path = $"api/bookings/user-bookings/{userId}";
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

        /// <summary>
        /// Creates the booking async.
        /// </summary>
        /// <param name="romId">The rom id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="description">The description.</param>
        /// <returns>A Task.</returns>
        public async Task<Booking> CreateBookingAsync(BookingLight booking)
        {
            var path = "api/bookings/";

            using (HttpClient client = CreateClient())
            {

                var response = await API_PostRequestAsync(client, path, booking);
                if (response.IsSuccessStatusCode)
                {
                    var result = (await response.ReadContentAsync<BookingDto>())?.BookingDtoAsOriginal();
                    return result;
                }

                string errorContent = await response.Content.ReadAsStringAsync();

                throw new Exception(errorContent);
            }
        }


        /// <summary>
        /// Deletes the room async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        public async Task<int> DeleteBookingAsync(int id, string userId)
        {
            var path = $"api/bookings/user-bookings/{id}/{userId}";
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
    }
}
