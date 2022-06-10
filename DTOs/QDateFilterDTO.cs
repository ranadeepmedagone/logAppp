using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
// using Nconnect.Utilities;

namespace logapp.DTOs
{
    public record QDateFilterDTO
    {
        private DateTimeOffset? _fromDate = null;
        private DateTimeOffset? _toDate = null;

        [FromQuery(Name = "from_date")]
        public DateTimeOffset? FromDate { get => _fromDate; set => _fromDate = value; }

        [FromQuery(Name = "to_date")]
        public DateTimeOffset? ToDate
        {
            // To Date must always be inclusive of the 'to date'
            get => this._toDate?.AddDays(1).Subtract(new TimeSpan(0, 0, 0, 0, 1));

            set => _toDate = value;
        }
    }

    public record QDateFilterSortingDTO : QDateFilterDTO
    {
        [FromQuery(Name = "sort_created_at")]
        public string SortCreatedAt { get; set; } = null;
    }

    public record QDateOfJoiningSortingDTO
    {
        [FromQuery(Name = "sort_by_date_of_joining")]
        public string SortByDateOfJoining { get; set; } = null;

    }
}