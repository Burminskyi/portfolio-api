namespace PortfolioApi.Models;

public class About
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;

    public string AvatarUrl { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string? TelegramUrl { get; set; }

    public string? GithubUrl { get; set; }

    public string? LinkedinUrl { get; set; }
}
