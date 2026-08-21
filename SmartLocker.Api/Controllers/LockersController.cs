using Microsoft.AspNetCore.Mvc;
using SmartLocker.Api.Models;
using SmartLocker.Api.Services;

namespace SmartLocker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LockersController : ControllerBase
{
    private readonly ILockerService _lockerService;

    public LockersController(ILockerService lockerService)
    {
        _lockerService = lockerService;
    }
    
    [HttpGet]
    public async Task<IEnumerable<Locker>> Get()
    {
        return await _lockerService.GetAllAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Locker>> Post(Locker locker)
    {
        var nuevoLocker = await _lockerService.AddAsync(locker);

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
    public async Task<ActionResult<Locker>> Put(int id, Locker locker)
    {
        var lockerActualizado = await _lockerService.UpdateAsync(id, locker);

        if (lockerActualizado == null)
        {
            return NotFound();
        }

        return Ok(lockerActualizado);
    }
}