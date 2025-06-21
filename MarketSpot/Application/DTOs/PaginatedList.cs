public class PaginatedList<T>
{
    public List<T> Items { get; set; } = new List<T>(); 
    public int TotalCount { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}
