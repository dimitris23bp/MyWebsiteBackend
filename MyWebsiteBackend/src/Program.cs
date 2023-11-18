using Microsoft.EntityFrameworkCore;
using MyWebsiteBackend;
using MyWebsiteBackend.Services;
using MyWebsiteBackend.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICertificatesService, CertificatesService>();

string connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION")!;
builder
    .Services
    .AddDbContext<MyDbContext>(
        options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 1, 0)))
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
