using System;

namespace Server.Ai.Api.Application.Queries;

public class AiProviderQueries(AiContext _aiContext) : IAiProviderQueries
{
    public async Task<List<AiProviderDto>> GetListAsync()
    {
        var providers = await _aiContext.AiProviders.AsQueryable().AsNoTracking().ToListAsync();
        return providers.Select(p => new AiProviderDto(){AiProviderId = p.Id, Name = p.Name, ApiKey = MaskApiKey(p.ApiKey)}).ToList();
    }
    private string MaskApiKey(string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey) || apiKey.Length <= 6)
            return apiKey;

        var maskLength = apiKey.Length - 6;
        return string.Concat(
            apiKey.Substring(0, 3),
            new string('*', maskLength),
            apiKey.Substring(apiKey.Length - 3)
        );
    }

}
