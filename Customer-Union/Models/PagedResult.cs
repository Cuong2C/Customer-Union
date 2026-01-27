namespace Customer_Union.Models
{
    public class PagedResult<T> where T : class
    {
        public List<T> Items { get; set; } = new List<T>();
        public object? NextCursor { get; set; }
        public object? PreviousCursor { get; set; }
        public bool HasMore { get; set; }
        public int pageSize { get; set; }
    }
}
