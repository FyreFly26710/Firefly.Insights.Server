namespace Server.Ai.Api.Application.Queries;

public interface IJobLogQueries
{
    Task<ExecutionLogDto?> GetExecutionLogByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Paged<JobLogDto>> GetJobLogsAsync(JobLogListRequest request, CancellationToken cancellationToken = default);
}
