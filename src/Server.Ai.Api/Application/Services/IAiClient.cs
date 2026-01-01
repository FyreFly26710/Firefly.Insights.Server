using System;

namespace Server.Ai.Api.Application.Services;

public interface IAiClient
{
    Task<string> GenerateArticleSummaryList(GenerateArticleSummaryRequest request);
}
