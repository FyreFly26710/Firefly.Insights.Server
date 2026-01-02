using System;

namespace Server.Ai.Api.Application.Queries;

public interface IAiModelQueries
{
    Task<List<AiModelDto>> GetListAsync(AiModelListRequest request, CancellationToken cancellationToken = default);
    Task<AiModelDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<List<LookupItemDto>> GetLookupList(CancellationToken cancellationToken = default);
}
