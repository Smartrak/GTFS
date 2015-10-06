﻿// The MIT License (MIT)

// Copyright (c) 2014 Ben Abelshausen

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

namespace GTFS
{
    /// <summary>
    /// Contains common extensions for this GTFS library.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Parses an integer number from a string at the given index and with the given length.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="idx"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int FastParse(this string value, int idx, int count)
        {
            int result = 0;
            int relIdx = count;
            for (int c = idx; c < idx + count; c++)
            {
                relIdx--;
                if (relIdx == 0)
                {
                    result = result + value[c].FastParse();
                    return result;
                }
                else if (relIdx == 1)
                {
                    result = result +
                        value[c].FastParse() * 10;
                }
                else if (relIdx == 2)
                {
                    result = result +
                        value[c].FastParse() * 100;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("value", "Value string too long, can be max 3 chars long.");
                }
            }
            return result;
        }

        /// <summary>
        /// Parses a number from a char value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int FastParse(this char value)
        {
            switch(value)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
            }
            throw new ArgumentOutOfRangeException("value", value.ToString());
        }

        /// <summary>
        /// Copies this feed to the given feed.
        /// </summary>
        /// <param name="thisFeed"></param>
        /// <param name="feed"></param>
        public static void CopyTo(this IGTFSFeed thisFeed, IGTFSFeed feed)
        {
            var feedInfo = thisFeed.GetFeedInfo();
            if(feedInfo != null)
            {
                feed.SetFeedInfo(feedInfo);
            }
            foreach(var entity in thisFeed.Agencies)
            {
                feed.Agencies.Add(entity);
            }
            foreach (var entity in thisFeed.CalendarDates)
            {
                feed.CalendarDates.Add(entity);
            }
            foreach (var entity in thisFeed.Calendars)
            {
                feed.Calendars.Add(entity);
            }
            foreach (var entity in thisFeed.FareAttributes)
            {
                feed.FareAttributes.Add(entity);
            }
            foreach (var entity in thisFeed.FareRules)
            {
                feed.FareRules.Add(entity);
            }
            foreach (var entity in thisFeed.Frequencies)
            {
                feed.Frequencies.Add(entity);
            }
            foreach (var entity in thisFeed.Routes)
            {
                feed.Routes.Add(entity);
            }
            foreach (var entity in thisFeed.Shapes)
            {
                feed.Shapes.Add(entity);
            }
            foreach (var entity in thisFeed.Stops)
            {
                feed.Stops.Add(entity);
            }
            foreach (var entity in thisFeed.StopTimes)
            {
                feed.StopTimes.Add(entity);
            }
            foreach (var entity in thisFeed.Transfers)
            {
                feed.Transfers.Add(entity);
            }
            foreach (var entity in thisFeed.Trips)
            {
                feed.Trips.Add(entity);
            }
        }

        /// <summary>
        /// Converts a number of milliseconds from 1/1/1970 into a standard DateTime.
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static DateTime FromUnixTime(this long milliseconds)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(milliseconds);
        }

        /// <summary>
        /// Converts a standard DateTime into the number of milliseconds since 1/1/1970.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long ToUnixTime(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalMilliseconds);
        }

        /// <summary>
        /// Calculates the distance in meter between the two given coordinates.
        /// </summary>
        /// <param name="latitude1"></param>
        /// <param name="longitude1"></param>
        /// <param name="latitude2"></param>
        /// <param name="longitude2"></param>
        /// <returns></returns>
        public static double DistanceInMeter(decimal latitude1, decimal longitude1, decimal latitude2, decimal longitude2)
        {
            var radius_earth = 6371000;

            var degToRandian = System.Math.PI / 180;
            var lat1_rad = (double)latitude1 * degToRandian;
            var lon1_rad = (double)longitude1 * degToRandian;
            var lat2_rad = (double)latitude2 * degToRandian;
            var lon2_rad = (double)longitude2 * degToRandian;
            var dLat = (lat2_rad - lat1_rad);
            var dLon = (lon2_rad - lon1_rad);
            var a = System.Math.Pow(System.Math.Sin(dLat / 2), 2) +
                System.Math.Cos(lat1_rad) * System.Math.Cos(lat2_rad) *
                System.Math.Pow(System.Math.Sin(dLon / 2), 2);
            var c = 2 * System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1 - a));
            var distance = radius_earth * c;
            return distance;
        }
    }
}
