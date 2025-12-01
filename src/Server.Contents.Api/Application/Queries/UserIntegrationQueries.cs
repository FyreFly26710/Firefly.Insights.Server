
namespace Server.Contents.Api.Application.Queries;

public class UserIntegrationQueries : IUserIntegrationQueries
{
    public async Task<UserTo> GetUserById(long userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<UserTo>> GetUserListByIds(List<long> userIds)
    {
        throw new NotImplementedException();
    }
}
