namespace PortfolioApi.DTOs;

public class ProjectDto
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? Link { get; set; }

    public List<string> SkillIds { get; set; } = new();
}
