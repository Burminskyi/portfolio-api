using PortfolioApi.DTOs;

namespace PortfolioApi.Services;

public interface ISkillService
{
    Task<IReadOnlyList<SkillDto>> GetAllAsync();

    Task<SkillDto?> GetByIdAsync(Guid id);

    Task<SkillDto> CreateAsync(CreateSkillDto dto);

    Task<SkillDto?> UpdateAsync(Guid id, UpdateSkillDto dto);

    Task<bool> DeleteAsync(Guid id);
}
