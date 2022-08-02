using System.Collections.Generic;

namespace Cars.COMMON.Responses
{
    public class PaginatedResponse<T>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int ItemsCount { get; set; }
        public List<T> Results { get; set; }
    }
}
