using System.ComponentModel.DataAnnotations;

namespace MyWebsiteBackend.Models;

public class Certificate
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Organization { get; set; }

    [Required]
    public DateTime IssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? CredendialId { get; set; } = null!;
    public Uri? CredendialUrl { get; set; } = null!;
}
