
namespace Server.Ai.Api.Application.Queries;

public class AiModelQueries(AiContext _aiContext, IMessageBus _messageBus) : IAiModelQueries
{
    public async Task<AiModelDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var query = _aiContext.AiModels.Include(x => x.AiProvider).AsQueryable().AsNoTracking();
        var aiModel = await query.Where(a => a.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (aiModel is null)
            return null;
        return aiModel.ToAiModelDto();
    }

    public async Task<List<AiModelDto>> GetListAsync(AiModelListRequest request, CancellationToken cancellationToken = default)
    {
        var query = _aiContext.AiModels.Include(x => x.AiProvider).AsQueryable().AsNoTracking();
        var aiModels = await query.ToListAsync(cancellationToken);
        
        var aiModelDtos = aiModels.Select(a => a.ToAiModelDto()).ToList();
        return aiModelDtos;
    }

    public async Task<List<LookupItemDto>> GetLookupList(CancellationToken cancellationToken = default)
    {
        var query = _aiContext.AiModels.AsQueryable().AsNoTracking();
        var aiModels = await query.ToListAsync(cancellationToken);
        return aiModels.Select(a => new LookupItemDto(a.Id, a.Model)).ToList();
    }
}
