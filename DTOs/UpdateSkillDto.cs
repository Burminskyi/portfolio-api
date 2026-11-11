using System.ComponentModel.DataAnnotations;

namespace PortfolioApi.DTOs;

public class UpdateSkillDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public string? Image { get; set; }

    [Required]
    public string Level { get; set; } = "beginner";

    public List<string> ProjectIds { get; set; } = new();
}
