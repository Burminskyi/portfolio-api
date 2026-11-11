using PortfolioApi.DTOs;

namespace PortfolioApi.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
}
