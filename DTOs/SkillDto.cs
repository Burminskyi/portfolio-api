namespace PortfolioApi.DTOs;

public class SkillDto
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? Image { get; set; }

    public string Level { get; set; } = "beginner";

    public List<string> ProjectIds { get; set; } = new();
}
