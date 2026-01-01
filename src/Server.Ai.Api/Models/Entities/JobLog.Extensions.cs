using System;

namespace Server.Ai.Api.Models.Entities;

public static class JobLogExtensions
{
    public static JobLogDto ToJobLogDto(this JobLog jobLog, UserTo userTo) => new JobLogDto()
    {
        JobLogId = jobLog.Id,
        UserId = jobLog.UserId,
        UserName = userTo.UserName,
        JobType = jobLog.JobType.ToString(),
        Status = jobLog.Status.ToString(),
        CreatedAt = jobLog.CreatedAt,
        StartedAt = jobLog.StartedAt,
        CompletedAt = jobLog.CompletedAt,
        AiModel = jobLog.AiModel?.ToAiModelDto() ?? new AiModelDto(),
    };
}
