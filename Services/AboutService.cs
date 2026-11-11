using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.DTOs;
using PortfolioApi.Models;

namespace PortfolioApi.Services;

public class AboutService(AppDbContext context) : IAboutService
{
    public async Task<AboutDto?> GetAsync()
    {
        var about = await context.Abouts.AsNoTracking().FirstOrDefaultAsync();
        return about is null ? null : MapToDto(about);
    }

    public async Task<AboutDto> UpdateAsync(AboutDto dto)
    {
        var about = await context.Abouts.FirstOrDefaultAsync();

        if (about is null)
        {
            about = new About { Id = Guid.NewGuid() };
            context.Abouts.Add(about);
        }

        about.FullName = dto.FullName;
        about.Title = dto.Title;
        about.Bio = dto.Bio;
        about.AvatarUrl = dto.AvatarUrl;
        about.Location = dto.Location;
        about.TelegramUrl = dto.SocialLinks.TelegramUrl;
        about.GithubUrl = dto.SocialLinks.GithubUrl;
        about.LinkedinUrl = dto.SocialLinks.LinkedinUrl;

        await context.SaveChangesAsync();
        return MapToDto(about);
    }

    private static AboutDto MapToDto(About about) => new()
    {
        FullName = about.FullName,
        Title = about.Title,
        Bio = about.Bio,
        AvatarUrl = about.AvatarUrl,
        Location = about.Location,
        SocialLinks = new SocialLinksDto
        {
            TelegramUrl = about.TelegramUrl,
            GithubUrl = about.GithubUrl,
            LinkedinUrl = about.LinkedinUrl
        }
    };
}
