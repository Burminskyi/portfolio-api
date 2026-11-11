using PortfolioApi.DTOs;

namespace PortfolioApi.Services;

public interface IAboutService
{
    Task<AboutDto?> GetAsync();

    Task<AboutDto> UpdateAsync(AboutDto dto);
}
