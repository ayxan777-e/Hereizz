namespace Application.Shared.Responses;

public class PagedResponse<T>
{
    public T Items { get; set; } = default!;

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public decimal TotalCount { get; set; }

    public int TotalPages { get; set; }
}