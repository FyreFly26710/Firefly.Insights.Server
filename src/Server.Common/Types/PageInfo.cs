namespace Server.Common.Types;
public record PageInfo
{
    /// <summary>
    /// Page number. Default: 1
    /// </summary>
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// Page size. Default: 20
    /// </summary>
    public int PageSize { get; init; } = 20;

    /// <summary>
    /// Sort field. Default: null, override by child record
    /// </summary>
    public virtual string? SortField { get; init; } = null;

    public virtual bool IsAscending { get; init; } = true;
}