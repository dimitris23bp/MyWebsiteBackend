using MyWebsiteBackend.Models;

namespace MyWebsiteBackend.Services.Interfaces;

public interface ICertificatesService
{
    public Task<Certificate?> GetCertificateById(int id);
    public Task<IEnumerable<Certificate>> GetCertificates();
}
