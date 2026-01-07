using System;
using Server.Common.Extensions;

namespace Server.Ai.Api.Application.Queries;

public class ExecutionLogQueries(AiContext _aiContext, IMessageBus _messageBus) : IExecutionLogQueries
{
    private IQueryable<ExecutionLog> GetQuery()
    {
        var query = _aiContext.ExecutionLogs.AsQueryable().AsNoTracking();
        query = query.Include(e => e.JobLog).ThenInclude(j => j.AiModel).ThenInclude(m => m.AiProvider);
        // query = query.Include(e => e.ExecutionPayload);
        return query;
    }
    public async Task<ExecutionLogDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var query = GetQuery();
        var executionLog = await query.Where(e => e.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (executionLog is null)
            throw new ExceptionNotFound("Execution log not found");
        
        var userResultMessage = await _messageBus.RequestAsync<UserRequestMessage, UserRequestMessageResponse>(new UserRequestMessage(executionLog.JobLog.UserId), cancellationToken);
        var jobLog = executionLog.JobLog.ToJobLogDto(userResultMessage.UserTo);
        return executionLog.ToExecutionLogDto(jobLog);
    }

    public async Task<Paged<ExecutionLogDto>> GetListAsync(ExecutionLogListRequest request, CancellationToken cancellationToken = default)
    {
        PageInfo pagedInfo = request;
        var query = GetQuery();
        if (request.UserId is not null)
            query = query.Where(e => e.JobLog.UserId == request.UserId);
        if (request.AiModelId is not null)
            query = query.Where(e => e.JobLog.AiModelId == request.AiModelId);
        if (request.IsSuccessful is not null)
            query = query.Where(e => e.IsSuccessful == request.IsSuccessful);
        var pagedData = await query.ToPagedAsync(pagedInfo);

        var userIds = pagedData.Data.Select(e => e.JobLog.UserId).Distinct().ToList();
        var userRequestMessages = await _messageBus.RequestAsync<UserListRequestMessage, UserListRequestMessageResponse>(new UserListRequestMessage(userIds), cancellationToken);
        var userTos = userRequestMessages.UserTos;
        var executionLogs = new List<ExecutionLogDto>();
        foreach (var executionLog in pagedData.Data)
        {
            var jobLog = executionLog.JobLog.ToJobLogDto(userTos.FirstOrDefault(u => u.UserId == executionLog.JobLog.UserId) ?? new UserTo(executionLog.JobLog.UserId));
            executionLogs.Add(executionLog.ToExecutionLogDto(jobLog));
        }
        return new Paged<ExecutionLogDto>(pagedInfo, pagedData.TotalCount, executionLogs);
    }
}
