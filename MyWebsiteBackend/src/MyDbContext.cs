using Microsoft.EntityFrameworkCore;
using MyWebsiteBackend.Models;

namespace MyWebsiteBackend;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options) { }

    public virtual DbSet<Certificate> Certificates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION")!;
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 1, 0)));
        }
    }
}
