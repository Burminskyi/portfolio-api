namespace PortfolioApi.DTOs;

public class AboutDto
{
    public string FullName { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;

    public string AvatarUrl { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public SocialLinksDto SocialLinks { get; set; } = new();
}
