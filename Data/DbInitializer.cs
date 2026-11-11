using Microsoft.EntityFrameworkCore;
using PortfolioApi.Models;

namespace PortfolioApi.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context, IConfiguration configuration, bool useSqlite)
    {
        if (useSqlite)
        {
            // SQLite for local development — create schema without provider-specific migrations.
            await context.Database.EnsureCreatedAsync();
        }
        else
        {
            await context.Database.MigrateAsync();
        }

        if (await context.Users.AnyAsync())
        {
            return;
        }

        var adminEmail = configuration["Seed:AdminEmail"] ?? "admin@portfolio.local";
        var adminPassword = configuration["Seed:AdminPassword"] ?? "Admin123!";

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Email = adminEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword),
            Role = "Admin"
        };

        var about = new About
        {
            Id = Guid.NewGuid(),
            FullName = "Anton Developer",
            Title = "Fullstack .NET / Angular Developer",
            Bio = "Passionate fullstack developer focused on clean architecture, modern .NET APIs and Angular SPA applications.",
            AvatarUrl = "/assets/avatar.png",
            Location = "Ukraine",
            TelegramUrl = "https://t.me/username",
            GithubUrl = "https://github.com/username",
            LinkedinUrl = "https://linkedin.com/in/username"
        };

        var csharp = new Skill
        {
            Id = Guid.NewGuid(),
            Title = "C# / .NET",
            Category = "Backend",
            Description = "Building REST APIs with ASP.NET Core, EF Core and clean architecture.",
            Image = null,
            Level = SkillLevel.Advanced
        };

        var angular = new Skill
        {
            Id = Guid.NewGuid(),
            Title = "Angular",
            Category = "Frontend",
            Description = "Modern Angular SPAs with standalone components and reactive forms.",
            Image = null,
            Level = SkillLevel.Intermediate
        };

        var postgres = new Skill
        {
            Id = Guid.NewGuid(),
            Title = "PostgreSQL",
            Category = "Database",
            Description = "Relational database design, migrations and performance basics.",
            Image = null,
            Level = SkillLevel.Intermediate
        };

        var portfolioProject = new Project
        {
            Id = Guid.NewGuid(),
            Title = "Dev Portfolio",
            Description = "Personal portfolio SPA with Angular frontend and .NET Web API backend.",
            Link = "https://github.com/username/dev-portfolio"
        };

        var apiProject = new Project
        {
            Id = Guid.NewGuid(),
            Title = "Portfolio API",
            Description = "REST API for portfolio content: about, skills, projects and JWT auth.",
            Link = "https://github.com/username/portfolio-api"
        };

        var links = new List<ProjectSkill>
        {
            new() { ProjectId = portfolioProject.Id, SkillId = angular.Id },
            new() { ProjectId = portfolioProject.Id, SkillId = csharp.Id },
            new() { ProjectId = apiProject.Id, SkillId = csharp.Id },
            new() { ProjectId = apiProject.Id, SkillId = postgres.Id }
        };

        context.Users.Add(admin);
        context.Abouts.Add(about);
        context.Skills.AddRange(csharp, angular, postgres);
        context.Projects.AddRange(portfolioProject, apiProject);
        context.ProjectSkills.AddRange(links);

        await context.SaveChangesAsync();
    }
}
