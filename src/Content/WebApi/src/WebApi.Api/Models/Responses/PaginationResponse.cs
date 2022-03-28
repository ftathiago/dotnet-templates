using System;
using System.Collections.Generic;

namespace WebApi.Api.Models.Responses
{
    [Serializable]
    public class PaginationResponse<TResponseData>
    {
        public PaginationResponse(
            IEnumerable<TResponseData> data,
            int currentPage,
            int totalItems,
            int pageSize)
        {
            Data = data;
            CurrentPage = 1;
            TotalItems = totalItems;
            TotalPages = 0;

            var hasRecords = totalItems > 0;

            if (hasRecords)
            {
                CurrentPage = currentPage;
                TotalPages = ((totalItems - 1) / pageSize) + 1;
            }
        }

        public IEnumerable<TResponseData> Data { get; }

        /// <summary>
        /// Requested page number.
        /// </summary>
        public int CurrentPage { get; }

        /// <summary>
        /// Total records that attends the provided criteria.
        /// </summary>
        public int TotalItems { get; }

        /// <summary>
        /// Based on _size and records found, this is the total pages.
        /// </summary>
        public int TotalPages { get; }
    }
}
