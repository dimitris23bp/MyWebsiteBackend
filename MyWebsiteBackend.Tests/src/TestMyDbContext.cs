using Microsoft.EntityFrameworkCore;

namespace MyWebsiteBackend.Tests;

public class TestMyDbContext : MyDbContext
{
    public TestMyDbContext(DbContextOptions<MyDbContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseInMemoryDatabase("TestDatabase");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
