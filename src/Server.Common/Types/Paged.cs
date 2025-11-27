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
    public PageInfo? PageInfo { get; set; } = null;

    public List<T>? Data { get; set; } = null;

}
