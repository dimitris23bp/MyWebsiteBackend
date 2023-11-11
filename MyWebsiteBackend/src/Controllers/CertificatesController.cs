using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace MyWebsiteBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class CertificatesController : Controller
{
    private readonly ILogger<CertificatesController> _logger;
    private readonly MyDbContext _dbContext;

    public CertificatesController(ILogger<CertificatesController> logger, MyDbContext myDbContext)
    {
        this._logger = logger;
        this._dbContext = myDbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Certificate>>> GetCertificates()
    {
        return await _dbContext.Certificates.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Certificate>> GetCertificate(int id)
    {
        var certificate = await _dbContext.Certificates.FindAsync(id);

        if (certificate == null)
        {
            _logger.LogDebug($"Certificate with id {id} was not found");
            return NotFound();
        }

        return certificate;
    }
}