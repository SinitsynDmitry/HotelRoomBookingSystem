/******************************************************************************
 *
 * File: DateRange.cs
 *
 * Description: DateRange.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HotelModels.Dtos
{
    public record DateRange
    {
        /// <summary>
        /// Gets the start time.
        /// </summary>
        public DateTimeOffset? StartTime { get; init; } = DateTimeOffset.MinValue;
        /// <summary>
        /// Gets the end time.
        /// </summary>
        public DateTimeOffset? EndTime { get; init; } = DateTimeOffset.MaxValue;

        public DateRange()
        {
            StartTime = DateTimeOffset.UtcNow;
            EndTime = StartTime.Value.AddDays(30);
        }

        /// <summary>
        /// Gets the days between.
        /// </summary>
        /// <returns>A list of DateTimeOffsets.</returns>
        public List<DateTimeOffset> GetDaysBetween()
        {
            List<DateTimeOffset> daysList = new List<DateTimeOffset>();
            var start = DateTimeOffset.UtcNow;
            if (StartTime.HasValue)
            {
                start = StartTime.Value;
            }
            var end = start.AddDays(60);

            if (EndTime.HasValue && EndTime.Value < end)
            {
                end = EndTime.Value;
            }

            if (start <= end)
            {
                // Iterate through the days and add to the list
                for (DateTimeOffset day = start; day <= end; day = day.AddDays(1))
                {
                    daysList.Add(day);
                }
            }

            return daysList;
        }

        /// <summary>
        /// Converts the daterange.
        /// </summary>
        /// <param name="daterange">The daterange.</param>
        /// <returns>A Tuple.</returns>
        public static Tuple<DateTimeOffset?, DateTimeOffset?> ConvertDaterange(string daterange)
        {
            DateTimeOffset? startDate = null;
            DateTimeOffset? endDate = null;

            if (!string.IsNullOrWhiteSpace(daterange))
            {
                string[] dateParts = daterange.Split("<->", StringSplitOptions.RemoveEmptyEntries);

                if (dateParts.Length > 1)
                {
                    // Parse the start date
                    try
                    {
                        startDate = DateTimeOffset.ParseExact(dateParts[0].Trim(), "dd.MM.yyyy", null);
                    }
                    catch { }
                }

                if (dateParts.Length == 2)
                {
                    // Parse the end date 
                    try
                    {
                        endDate = DateTimeOffset.ParseExact(dateParts[1].Trim(), "dd.MM.yyyy", null);
                    }
                    catch { }
                }
            }
            return new Tuple<DateTimeOffset?, DateTimeOffset?>(startDate, endDate);
        }


        /// <summary>
        /// Tos the string.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return StartTime?.ToString("dd.MM.yyyy") + " <-> " + EndTime?.ToString("dd.MM.yyyy");
        }
    }
}
