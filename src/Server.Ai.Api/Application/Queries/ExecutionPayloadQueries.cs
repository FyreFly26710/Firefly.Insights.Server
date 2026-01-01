using System;

namespace Server.Ai.Api.Application.Queries;

public class ExecutionPayloadQueries(AiContext _aiContext) : IExecutionPayloadQueries
{
    public async Task<ExecutionPayloadDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var executionPayload = await _aiContext.ExecutionPayloads.AsQueryable().AsNoTracking().Where(e => e.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (executionPayload is null)
            throw new ExceptionNotFound("Execution payload not found");
        return executionPayload.ToExecutionPayloadDto();
    }
}
