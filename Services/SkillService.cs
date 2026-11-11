using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.DTOs;
using PortfolioApi.Helpers;
using PortfolioApi.Models;

namespace PortfolioApi.Services;

public class SkillService(AppDbContext context) : ISkillService
{
    public async Task<IReadOnlyList<SkillDto>> GetAllAsync()
    {
        var skills = await context.Skills
            .AsNoTracking()
            .Include(s => s.ProjectSkills)
            .OrderBy(s => s.Title)
            .ToListAsync();

        return skills.Select(MapToDto).ToList();
    }

    public async Task<SkillDto?> GetByIdAsync(Guid id)
    {
        var skill = await context.Skills
            .AsNoTracking()
            .Include(s => s.ProjectSkills)
            .FirstOrDefaultAsync(s => s.Id == id);

        return skill is null ? null : MapToDto(skill);
    }

    public async Task<SkillDto> CreateAsync(CreateSkillDto dto)
    {
        var skill = new Skill
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Category = dto.Category,
            Description = dto.Description,
            Image = dto.Image,
            Level = SkillLevelParser.Parse(dto.Level)
        };

        await SyncProjectLinksAsync(skill, dto.ProjectIds);

        context.Skills.Add(skill);
        await context.SaveChangesAsync();

        return MapToDto(skill);
    }

    public async Task<SkillDto?> UpdateAsync(Guid id, UpdateSkillDto dto)
    {
        var skill = await context.Skills
            .Include(s => s.ProjectSkills)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (skill is null)
        {
            return null;
        }

        skill.Title = dto.Title;
        skill.Category = dto.Category;
        skill.Description = dto.Description;
        skill.Image = dto.Image;
        skill.Level = SkillLevelParser.Parse(dto.Level);

        context.ProjectSkills.RemoveRange(skill.ProjectSkills);
        skill.ProjectSkills.Clear();
        await SyncProjectLinksAsync(skill, dto.ProjectIds);

        await context.SaveChangesAsync();
        return MapToDto(skill);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var skill = await context.Skills.FirstOrDefaultAsync(s => s.Id == id);
        if (skill is null)
        {
            return false;
        }

        context.Skills.Remove(skill);
        await context.SaveChangesAsync();
        return true;
    }

    private async Task SyncProjectLinksAsync(Skill skill, IEnumerable<string> projectIds)
    {
        var ids = ParseGuids(projectIds);
        if (ids.Count == 0)
        {
            return;
        }

        var existing = await context.Projects
            .Where(p => ids.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync();

        foreach (var projectId in existing)
        {
            skill.ProjectSkills.Add(new ProjectSkill
            {
                SkillId = skill.Id,
                ProjectId = projectId
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

    private static SkillDto MapToDto(Skill skill) => new()
    {
        Id = skill.Id.ToString(),
        Title = skill.Title,
        Category = skill.Category,
        Description = skill.Description,
        Image = skill.Image,
        Level = SkillLevelParser.ToApiString(skill.Level),
        ProjectIds = skill.ProjectSkills.Select(ps => ps.ProjectId.ToString()).ToList()
    };
}
