namespace Shared.Models;

public sealed record PaginationParams
{
    public PaginationParams()
    {
    }

    public PaginationParams(int offset, int limit)
    {
        Limit = limit;
        Offset = offset;
    }

    [JsonRequired]
    public int Offset { get; set; }

    [JsonRequired]
    public int Limit { get; set; }
}
