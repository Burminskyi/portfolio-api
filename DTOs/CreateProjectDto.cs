using System.ComponentModel.DataAnnotations;

namespace PortfolioApi.DTOs;

public class CreateProjectDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public string? Link { get; set; }

    public List<string> SkillIds { get; set; } = new();
}
