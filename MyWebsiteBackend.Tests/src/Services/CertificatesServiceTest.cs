namespace MyWebsiteBackend.Tests.Services;

using System.Linq;
using MyWebsiteBackend.Models;
using MyWebsiteBackend.Services;
using MyWebsiteBackend.Tests;
using Xunit;

public class CertificatesServiceTests : IDisposable, IClassFixture<TestFixture>
{
    public readonly TestMyDbContext _context;

    public CertificatesServiceTests(TestFixture fixture)
    {
        _context = fixture.Context;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Certificates.RemoveRange(_context.Certificates);
            _context.SaveChanges();
        }
    }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task GetCertificates_ReturnsAllCertificates()
    {
        var data = GenerateCertificates();

        _context.Certificates.AddRange(data);
        _context.SaveChanges();

        var service = new CertificatesService(_context);

        // Act
        var certificates = await service.GetCertificates();

        // Assert
        Assert.Equal(3, certificates.Count());
    }

    [Fact]
    public async Task GetCertificate_ReturnsCertificateById()
    {
        var data = GenerateCertificates();
        var id = data.First().Id;

        _context.Certificates.AddRange(data);
        _context.SaveChanges();

        var service = new CertificatesService(_context);

        // Act
        var certificate = await service.GetCertificateById(id);

        // Assert
        Assert.Equal(id, certificate!.Id);
    }

    private IEnumerable<Certificate> GenerateCertificates()
    {
        return new List<Certificate>
        {
            new Certificate
            {
                Id = 1,
                Name = "Certificate 1",
                Organization = "Organization 1",
                IssueDate = DateTime.Now
            },
            new Certificate
            {
                Id = 2,
                Name = "Certificate 2",
                Organization = "Organization 2",
                IssueDate = DateTime.Now
            },
            new Certificate
            {
                Id = 3,
                Name = "Certificate 3",
                Organization = "Organization 3",
                IssueDate = DateTime.Now
            },
        };
    }
}
