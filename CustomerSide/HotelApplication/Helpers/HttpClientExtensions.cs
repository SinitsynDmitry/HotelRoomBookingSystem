/******************************************************************************
 *
 * File: HttpClientExtensions.cs
 *
 * Description: HttpClientExtensions.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using System.Text.Json;

namespace HotelApplication.Helpers
{
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Reads the content async.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>A Task.</returns>
        public static async Task<T> ReadContentAsync<T>(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode == false)
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var result = JsonSerializer.Deserialize<T>(
                dataAsString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result;
        }
    }
}
