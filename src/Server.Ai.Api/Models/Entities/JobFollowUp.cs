using System;

namespace Server.Ai.Api.Models.Entities;

public class JobFollowUp : Entity
{
    public long ParentJobLogId { get; set; }
    public long JobLogId { get; set; }

    public JobLog? JobLog { get; set; }
}
