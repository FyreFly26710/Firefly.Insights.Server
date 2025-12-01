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
    public PageRequest PageRequest { get; set; } = new();
    public int TotalCount { get; set; } = 0;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageRequest.PageSize);

    public List<T> Data { get; set; } = new();

    public Paged() { }
    public Paged(PageRequest pageRequest, int totalCount, List<T> data)
    {
        PageRequest = pageRequest;
        TotalCount = totalCount;
        Data = data;
    }
}
