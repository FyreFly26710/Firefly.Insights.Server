using System;

namespace Server.Ai.Api.Models.Entities;

public static class ExecutionPayloadExtensions
{
    public static ExecutionPayloadDto ToExecutionPayloadDto(this ExecutionPayload executionPayload) => new ExecutionPayloadDto(
        executionPayload.Id,
        executionPayload.Prompt,
        executionPayload.Response
    );
}
