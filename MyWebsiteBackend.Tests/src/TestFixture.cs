using Microsoft.EntityFrameworkCore;

namespace MyWebsiteBackend.Tests;

public class TestFixture
{
    public TestMyDbContext Context { get; private set; }

    public TestFixture()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().Options;

        Context = new TestMyDbContext(options);
    }
}
