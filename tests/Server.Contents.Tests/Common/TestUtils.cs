using Server.Contents.Api.Infrastructure.EfContexts;
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
}
