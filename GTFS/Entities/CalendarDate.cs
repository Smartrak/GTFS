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

using GTFS.Attributes;
using GTFS.Entities.Enumerations;
using System;

namespace GTFS.Entities
{
    /// <summary>
    /// Represents exceptions for the service IDs defined in the calendar. If a CalendarDate exists for ALL dates of service, they may be use instead of Calendar.
    /// </summary>
    [FileName("calendar_date")]
    public class CalendarDate : GTFSEntity
    {
        /// <summary>
        /// Gets or sets an ID that uniquely identifies a set of dates when a service exception is available for one or more routes. Each (service_id, date) pair can only appear once in calendar_dates.txt. If the a service_id value appears in both the calendar.txt and calendar_dates.txt files, the information in calendar_dates.txt modifies the service information specified in calendar.txt. This field is referenced by the trips.txt file.
        /// </summary>
        [Required]
        [FieldName("service_id")]
        public string ServiceId { get; set; }

        /// <summary>
        /// Gets or sets a particular date when service availability is different than the norm. You can use the exception_type field to indicate whether service is available on the specified date.
        /// </summary>
        [Required]
        [FieldName("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the exception type that indicates whether service is available on the date specified in the date field.
        /// </summary>
        [Required]
        [FieldName("exception_type")]
        public ExceptionType ExceptionType { get; set; }

        /// <summary>
        /// Returns a description of this trip.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[{0}] {1} {2}", this.ServiceId, this.Date, this.ExceptionType.ToString());
        }
    }
}