namespace GoodHamburger.Application.DTOs.Common;

public sealed class PaginationRequest
{
    private const int MaxPageSize = 100;

    public int Page { get; init; }
    public int PageSize { get; init; }

    public int Skip => (Page - 1) * PageSize;
    public int Take => PageSize > MaxPageSize ? MaxPageSize : PageSize;

    public static PaginationRequest Create(int page, int pageSize)
    {
        return new PaginationRequest
        {
            Page = page < 1 ? 1 : page,
            PageSize = pageSize < 1 ? 10 : pageSize
        };
    }
}