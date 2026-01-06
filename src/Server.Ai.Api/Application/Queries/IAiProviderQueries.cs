using System;

namespace Server.Ai.Api.Application.Queries;

public interface IAiProviderQueries
{
    Task<List<AiProviderDto>> GetListAsync();
    // Task<AiProviderDto> GetByIdAsync(long id);
}
