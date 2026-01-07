using System;
using Server.Common.Utils;

namespace Server.Ai.Api.Models.Entities;

public static class AiModelExtensions
{
    public static AiModelDto ToAiModelDto(this AiModel aiModel) => new AiModelDto()
    {
        AiModelId = aiModel.Id,
        Provider = aiModel.AiProvider.Name,
        AiProviderId = aiModel.AiProviderId,
        Model = aiModel.Model,
        ModelId = aiModel.ModelId,
        InputPrice = aiModel.InputPrice,
        OutputPrice = aiModel.OutputPrice,
        IsActive = aiModel.IsActive,
        DisplayName = aiModel.DisplayName,
        Avatar = aiModel.Avatar,
        Description = aiModel.Description,
    };
}
