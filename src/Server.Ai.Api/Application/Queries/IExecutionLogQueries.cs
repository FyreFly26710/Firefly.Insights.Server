using System;

namespace Server.Ai.Api.Application.Queries;

public interface IExecutionLogQueries
{
    Task<ExecutionLogDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Paged<ExecutionLogDto>> GetListAsync(ExecutionLogListRequest request, CancellationToken cancellationToken = default);

}
