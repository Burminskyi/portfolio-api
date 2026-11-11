using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.DTOs;
using PortfolioApi.Models;

namespace PortfolioApi.Services;

public class ProjectService(AppDbContext context) : IProjectService
{
    public async Task<IReadOnlyList<ProjectDto>> GetAllAsync()
    {
        var projects = await context.Projects
            .AsNoTracking()
            .Include(p => p.ProjectSkills)
            .OrderBy(p => p.Title)
            .ToListAsync();

        return projects.Select(MapToDto).ToList();
    }

    public async Task<ProjectDto?> GetByIdAsync(Guid id)
    {
        var project = await context.Projects
            .AsNoTracking()
            .Include(p => p.ProjectSkills)
            .FirstOrDefaultAsync(p => p.Id == id);

        return project is null ? null : MapToDto(project);
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Link = dto.Link
        };

        await SyncSkillLinksAsync(project, dto.SkillIds);

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        return MapToDto(project);
    }

    public async Task<ProjectDto?> UpdateAsync(Guid id, UpdateProjectDto dto)
    {
        var project = await context.Projects
            .Include(p => p.ProjectSkills)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
        {
            return null;
        }

        project.Title = dto.Title;
        project.Description = dto.Description;
        project.Link = dto.Link;

        context.ProjectSkills.RemoveRange(project.ProjectSkills);
        project.ProjectSkills.Clear();
        await SyncSkillLinksAsync(project, dto.SkillIds);

        await context.SaveChangesAsync();
        return MapToDto(project);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var project = await context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        if (project is null)
        {
            return false;
        }

        context.Projects.Remove(project);
        await context.SaveChangesAsync();
        return true;
    }

    private async Task SyncSkillLinksAsync(Project project, IEnumerable<string> skillIds)
    {
        var ids = ParseGuids(skillIds);
        if (ids.Count == 0)
        {
            return;
        }

        var existing = await context.Skills
            .Where(s => ids.Contains(s.Id))
            .Select(s => s.Id)
            .ToListAsync();

        foreach (var skillId in existing)
        {
            project.ProjectSkills.Add(new ProjectSkill
            {
                ProjectId = project.Id,
                SkillId = skillId
            });
        }
    }

    private static List<Guid> ParseGuids(IEnumerable<string> values) =>
        values
            .Select(v => Guid.TryParse(v, out var id) ? id : (Guid?)null)
            .Where(id => id.HasValue)
            .Select(id => id!.Value)
            .Distinct()
            .ToList();

    private static ProjectDto MapToDto(Project project) => new()
    {
        Id = project.Id.ToString(),
        Title = project.Title,
        Description = project.Description,
        Link = project.Link,
        SkillIds = project.ProjectSkills.Select(ps => ps.SkillId.ToString()).ToList()
    };
}
