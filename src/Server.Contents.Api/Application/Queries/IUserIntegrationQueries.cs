namespace Server.Contents.Api.Application.Queries;

public interface IUserIntegrationQueries
{
    public Task<UserTo> GetUserById(long userId);
    public Task<List<UserTo>> GetUserListByIds(List<long> userIds);

}


public class UserTo
{
    public long UserId { get; set; }
    public string UserName { get; set; }
}
