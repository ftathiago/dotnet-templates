using System.Collections.Generic;

namespace WebApi.Domain.Repositories
{
    public struct PaginatedResult<T>
        where T : class
    {
        public PaginatedResult(int totalItems, IEnumerable<T> data)
        {
            Data = data;

            TotalItems = totalItems;
        }

        public int TotalItems { get; init; }

        public IEnumerable<T> Data { get; init; }
    }
}
