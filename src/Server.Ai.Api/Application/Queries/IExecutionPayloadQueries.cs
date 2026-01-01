using System;

namespace Server.Ai.Api.Application.Queries;

public interface IExecutionPayloadQueries
{
    Task<ExecutionPayloadDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

}
