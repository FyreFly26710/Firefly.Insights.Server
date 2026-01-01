using System;
using Server.Messages.Ais;

namespace Server.Ai.Api.Application.Services;

public interface IArticleGenerationClient
{
    Task GenerateArticleSummaryListAsync(GenerateArticleSummaryMessage message, CancellationToken cancellationToken = default);
}
