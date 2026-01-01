using System;

namespace Server.Ai.Api.Models.Entities;

public static class AiModelExtensions
{
    public static AiModelDto ToAiModelDto(this AiModel aiModel) => new AiModelDto()
    {
        AiModelId = aiModel.Id,
        Provider = aiModel.Provider,
        Model = aiModel.Model,
        ModelId = aiModel.ModelId,
        InputPrice = aiModel.InputPrice,
        OutputPrice = aiModel.OutputPrice,
        IsActive = aiModel.IsActive,
    };
}
