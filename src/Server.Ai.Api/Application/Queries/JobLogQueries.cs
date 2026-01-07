
using Server.Common.Extensions;

namespace Server.Ai.Api.Application.Queries;

public class JobLogQueries(AiContext _aiContext, IMessageBus _messageBus) : IJobLogQueries
{
    private IQueryable<JobLog> GetQuery()
    {
        var query = _aiContext.JobLogs.AsQueryable().AsNoTracking();
        query = query.Include(j => j.ExecutionLog);
        query = query.Include(j => j.AiModel).ThenInclude(m => m.AiProvider);
        return query;
    }
    public async Task<Paged<JobLogDto>> GetJobLogsAsync(JobLogListRequest request, CancellationToken cancellationToken = default)
    {
        PageInfo pagedInfo = request;
        var query = GetQuery();
        if (request.UserId is not null)
            query = query.Where(j => j.UserId == request.UserId);
        if (request.AiModelId is not null)
            query = query.Where(j => j.AiModelId == request.AiModelId);
        if (request.JobType is not null)
            query = query.Where(j => j.JobType == request.JobType);
        if (request.Status is not null)
            query = query.Where(j => j.Status == request.Status);

        var pagedData = await query.ToPagedAsync(pagedInfo);

        var userIds = pagedData.Data.Select(j => j.UserId).Distinct().ToList();
        var userRequestMessages = await _messageBus.RequestAsync<UserListRequestMessage, UserListRequestMessageResponse>(new UserListRequestMessage(userIds), cancellationToken);
        var userTos = userRequestMessages.UserTos;

        var jobLogs = pagedData.Data.Select(j => j.ToJobLogDto(userTos.FirstOrDefault(u => u.UserId == j.UserId) ?? new UserTo(j.UserId))).ToList();
        return new Paged<JobLogDto>(pagedInfo, pagedData.TotalCount, jobLogs);
    }
    public async Task<ExecutionLogDto?> GetExecutionLogByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var query = _aiContext.ExecutionLogs.AsQueryable().AsNoTracking();
        query = query.Include(e => e.JobLog);
        // query = query.Include(e => e.ExecutionPayload);
        var executionLog = await query.Where(e => e.JobLogId == id).FirstOrDefaultAsync(cancellationToken);
        if (executionLog is null)
            throw new ExceptionNotFound("Execution log not found");

        var userResultMessage = await _messageBus.RequestAsync<UserRequestMessage, UserRequestMessageResponse>(new UserRequestMessage(executionLog.JobLog.UserId), cancellationToken);
        var jobLogDto = executionLog.JobLog.ToJobLogDto(userResultMessage.UserTo);
        var executionLogDto = executionLog.ToExecutionLogDto(jobLogDto);
        return executionLogDto;
    }
}
