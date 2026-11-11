using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.DTOs;
using PortfolioApi.Services;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/skills")]
public class SkillsController(ISkillService skillService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<SkillDto>>> GetAll()
    {
        return Ok(await skillService.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<SkillDto>> GetById(Guid id)
    {
        var skill = await skillService.GetByIdAsync(id);
        return skill is null ? NotFound() : Ok(skill);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<SkillDto>> Create([FromBody] CreateSkillDto dto)
    {
        try
        {
            var created = await skillService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<SkillDto>> Update(Guid id, [FromBody] UpdateSkillDto dto)
    {
        try
        {
            var updated = await skillService.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await skillService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
