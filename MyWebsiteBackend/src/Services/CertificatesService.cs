using Microsoft.EntityFrameworkCore;
using MyWebsiteBackend.Models;
using MyWebsiteBackend.Services.Interfaces;

namespace MyWebsiteBackend.Services;

public class CertificatesService : ICertificatesService
{
    private readonly MyDbContext _dbContext;

    public CertificatesService(MyDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<Certificate?> GetCertificateById(int id)
    {
        return await _dbContext.Certificates.FindAsync(id);
    }

    public async Task<IEnumerable<Certificate>> GetCertificates()
    {
        return await _dbContext.Certificates.ToListAsync();
    }
}
