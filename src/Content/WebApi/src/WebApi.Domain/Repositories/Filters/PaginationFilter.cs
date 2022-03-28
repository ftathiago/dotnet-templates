namespace WebApi.Domain.Repositories.Filters
{
    public struct PaginationFilter
    {
        public string OrderBy { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
