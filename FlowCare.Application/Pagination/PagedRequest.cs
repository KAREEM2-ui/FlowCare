namespace FlowCare.Application.DTOs.Pagination;

public sealed record PagedRequest(int PageNumber = 1, int PageSize = 20)
{
    public int Skip => (PageNumber <= 1 ? 0 : (PageNumber - 1) * PageSize);
}
