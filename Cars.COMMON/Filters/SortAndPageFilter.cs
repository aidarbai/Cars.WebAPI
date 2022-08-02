namespace Cars.COMMON.Filters
{
    public class SortAndPageFilter
    {
        public string Attribute { get; set; } = "price";
        public string Order { get; set; } = "asc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}