using System;

namespace Server.Ai.Api.Models.Responses;

public record ExecutionPayloadDto(
    long ExecutionPayloadId,
    string Prompt,
    string? Response
);
