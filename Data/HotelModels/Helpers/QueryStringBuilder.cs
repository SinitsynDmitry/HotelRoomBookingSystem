/******************************************************************************
 *
 * File: QueryStringBuilder.cs
 *
 * Description: QueryStringBuilder.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelModels.Dtos;
using HotelModels.Select;
using System.Web;


namespace HotelModels.Helpers
{
    public class QueryStringBuilder
    {
        /// <summary>
        /// Builds the query string.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A string.</returns>
        public static string BuildQueryString(Rooms_SelectAndOrder request)
        {
            if (request == null) return string.Empty;

            var parameters = new List<string>();

            if (request.DateRange?.StartTime.HasValue == true)
                parameters.Add($"DateRange.StartTime={request.DateRange.StartTime.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}");

            if (request.DateRange?.EndTime.HasValue == true)
                parameters.Add($"DateRange.EndTime={request.DateRange.EndTime.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}");

            if (request.MinBeds.HasValue)
                parameters.Add($"minBeds={request.MinBeds}");

            if (request.MaxBeds.HasValue)
                parameters.Add($"maxBeds={request.MaxBeds}");

            if (request.MinPrice.HasValue)
                parameters.Add($"minPrice={request.MinPrice}");

            if (request.MaxPrice.HasValue)
                parameters.Add($"maxPrice={request.MaxPrice}");

            return string.Join("&", parameters);
        }

        /// <summary>
        /// Builds the query string.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A string.</returns>
        public static string BuildQueryString(DateRange request)
        {
            if(request==null) return string.Empty;

            var parameters = new List<string>();

            if (request.StartTime.HasValue == true)
                parameters.Add($"StartTime={request.StartTime.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}");

            if (request.EndTime.HasValue == true)
                parameters.Add($"EndTime={request.EndTime.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}");

            return string.Join("&", parameters);
        }

        /// <summary>
        /// Urls the encode.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A string.</returns>
        private static string UrlEncode(object value)
        {
            if (value is null) return string.Empty;

            return HttpUtility.UrlEncode(value.ToString());
        }
    }
}
