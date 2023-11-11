using Microsoft.EntityFrameworkCore;
using Models;

namespace MyWebsiteBackend;

public class MyDbContext : DbContext
{
    public DbSet<Certificate> Certificates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION")!;
        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 1, 0)));
    }

}