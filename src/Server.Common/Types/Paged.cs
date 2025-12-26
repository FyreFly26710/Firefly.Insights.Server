using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Common.Types;
/// <summary>
/// Paged response wrapper
/// </summary>
public class Paged<T> where T : class
{
    public PageInfo PageInfo { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageInfo.PageSize);
    public List<T> Data { get; init; }

    public Paged(PageInfo pageInfo, int totalCount, List<T> data)
    {
        PageInfo = pageInfo;
        TotalCount = totalCount;
        Data = data;
    }
}
