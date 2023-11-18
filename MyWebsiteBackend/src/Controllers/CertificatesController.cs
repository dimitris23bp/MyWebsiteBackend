using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebsiteBackend.Models;
using MyWebsiteBackend.Services;
using MyWebsiteBackend.Services.Interfaces;

namespace MyWebsiteBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class CertificatesController : Controller
{
    private readonly ILogger<CertificatesController> _logger;
    private readonly MyDbContext _dbContext;
    private readonly ICertificatesService _certificatesService;

    public CertificatesController(
        ILogger<CertificatesController> logger,
        MyDbContext myDbContext,
        ICertificatesService certificatesService
    )
    {
        this._logger = logger;
        this._dbContext = myDbContext;
        this._certificatesService = certificatesService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Certificate>>> GetCertificates()
    {
        var certificates = await _certificatesService.GetCertificates();

        if (certificates == null)
        {
            _logger.LogDebug("No certificates found");
            return Ok();
        }

        return certificates.ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Certificate>> GetCertificate(int id)
    {
        var certificate = await _certificatesService.GetCertificateById(id);

        if (certificate == null)
        {
            _logger.LogWarning($"Certificate with id {id} was not found");
            return NotFound();
        }

        return certificate;
    }
}
