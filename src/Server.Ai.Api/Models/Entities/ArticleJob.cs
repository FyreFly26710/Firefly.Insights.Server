using System;

namespace Server.Ai.Api.Models.Entities;

public class ArticleJob : Entity
{
    public long JobLogId { get; set; }
    public long ArticleId { get; set; }
    public JobLog? JobLog { get; set; }
}
