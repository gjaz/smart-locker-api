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
    public IEnumerable<Locker> Get()
    {
        return _lockerService.GetAll();
    }

    [HttpPost]
    public ActionResult<Locker> Post(Locker locker)
    {
        var nuevoLocker = _lockerService.Add(locker);

        return Ok(nuevoLocker);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var eliminado = _lockerService.Delete(id);

        if (!eliminado)
        {
            return NotFound();
        }

        return NoContent();
    }   
}