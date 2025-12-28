using Server.Contents.Api.Infrastructure.EfContexts;
using StackExchange.Redis;
using System;

namespace Server.Contents.Tests.Common;

public static class TestUtils
{
    public static ContentsContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ContentsContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ContentsContext(options);
    }

    private const int TestDbIndex = 15;
    public static IConnectionMultiplexer CreateTestRedisDb()
    {
        return ConnectionMultiplexer.Connect($"localhost:6380,allowAdmin=true,defaultDatabase={TestDbIndex}");
    }
    public static void ClearTestRedisDb(this IConnectionMultiplexer redis)
    {
        var server = redis.GetServer("localhost:6380");
        server.FlushDatabase(TestDbIndex);
    }
}
