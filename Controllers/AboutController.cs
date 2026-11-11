using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.DTOs;
using PortfolioApi.Services;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/about")]
public class AboutController(IAboutService aboutService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<AboutDto>> Get()
    {
        var about = await aboutService.GetAsync();
        if (about is null)
        {
            return NotFound(new { message = "About profile not found." });
        }

        return Ok(about);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult<AboutDto>> Update([FromBody] AboutDto dto)
    {
        var updated = await aboutService.UpdateAsync(dto);
        return Ok(updated);
    }
}
