using System;
using WebApi.Domain.Repositories.Filters;

namespace WebApi.DapperInfraData.Services
{
    internal static class SqlPagination
    {
        internal const int MinPageNumber = 0;
        internal const int MinPageSize = 1;
        internal const int MaxPageSize = 255;

        private static readonly string _pageNumberRangeError =
            $"Page number must be between 0 and {int.MaxValue}";

        private static readonly string _pageSizeRangeError =
            $"Page size must be between {MinPageSize} and {MaxPageSize}";

        public static string GetPagination(int pageNumber, int pageSize) =>
             From(new PaginationFilter { PageNumber = pageNumber, PageSize = pageSize });

        public static string From(PaginationFilter pagination)
        {
            if (pagination.PageNumber < MinPageNumber)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(pagination),
                    _pageNumberRangeError);
            }

            if (pagination.PageSize < MinPageSize || pagination.PageSize > MaxPageSize)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(pagination),
                    _pageSizeRangeError);
            }

            return $"LIMIT {pagination.PageSize} OFFSET {pagination.PageNumber * pagination.PageSize}";
        }
    }
}
