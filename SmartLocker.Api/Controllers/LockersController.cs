using Microsoft.AspNetCore.Mvc;
using SmartLocker.Api.Models;
using SmartLocker.Api.Services;
using SmartLocker.Api.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace SmartLocker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LockersController : ControllerBase
{
    private readonly ILockerService _lockerService;

    public LockersController(ILockerService lockerService)
    {
        _lockerService = lockerService;
    }

    [HttpGet]
    public async Task<IEnumerable<LockerDto>> Get()
    {
        return await _lockerService.GetAllAsync();
    }

    [HttpPost]
    public async Task<ActionResult<LockerDto>> Post(CreateLockerDto dto)
    {
        var nuevoLocker = await _lockerService.AddAsync(dto);

        return Ok(nuevoLocker);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _lockerService.DeleteAsync(id);

        if (!eliminado)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LockerDto>> Put(int id, UpdateLockerDto dto)
    {
        var actualizado = await _lockerService.UpdateAsync(id, dto);

        if (actualizado == null)
        {
            return NotFound();
        }

        return Ok(actualizado);
    }
}