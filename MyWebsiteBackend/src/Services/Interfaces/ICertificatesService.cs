using MyWebsiteBackend.Models;

namespace MyWebsiteBackend.Services.Interfaces;

public interface ICertificatesService
{
    public Task<Certificate?> GetCertificateById(int id);
    public Task<IEnumerable<Certificate>> GetCertificates();
    // Task<Certificate> CreateCertificate(Certificate certificate);
    // Task<Certificate> UpdateCertificate(int id, Certificate certificate);
    // Task<Certificate> DeleteCertificate(int id);
}
