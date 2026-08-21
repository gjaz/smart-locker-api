using Microsoft.EntityFrameworkCore;
using SmartLocker.Api.Data;
using SmartLocker.Api.Models;
using SmartLocker.Api.DTOs;

namespace SmartLocker.Api.Services;

public class LockerService : ILockerService
{
    private readonly SmartLockerDbContext _context;

    public LockerService(SmartLockerDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LockerDto>> GetAllAsync()
    {
        return await _context.Lockers
            .Select(locker => new LockerDto
            {
                Id = locker.Id,
                Codigo = locker.Codigo,
                Ubicacion = locker.Ubicacion,
                Estado = locker.Estado,
                Tamano = locker.Tamano
            })
            .ToListAsync();
    }

    public async Task<LockerDto> AddAsync(CreateLockerDto dto)
    {
        var locker = new Locker
        {
            Codigo = dto.Codigo,
            Ubicacion = dto.Ubicacion,
            Estado = dto.Estado,
            Tamano = dto.Tamano
        };

        _context.Lockers.Add(locker);

        await _context.SaveChangesAsync();

        return new LockerDto
        {
            Id = locker.Id,
            Codigo = locker.Codigo,
            Ubicacion = locker.Ubicacion,
            Estado = locker.Estado,
            Tamano = locker.Tamano
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var locker = await _context.Lockers
            .FirstOrDefaultAsync(x => x.Id == id);

        if (locker == null)
        {
            return false;
        }

        _context.Lockers.Remove(locker);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<LockerDto?> UpdateAsync(int id, UpdateLockerDto dto)
    {
        var lockerExistente = await _context.Lockers
            .FirstOrDefaultAsync(x => x.Id == id);

        if (lockerExistente == null)
        {
            return null;
        }

        lockerExistente.Codigo = dto.Codigo;
        lockerExistente.Ubicacion = dto.Ubicacion;
        lockerExistente.Estado = dto.Estado;
        lockerExistente.Tamano = dto.Tamano;

        await _context.SaveChangesAsync();

        return new LockerDto
        {
            Id = lockerExistente.Id,
            Codigo = lockerExistente.Codigo,
            Ubicacion = lockerExistente.Ubicacion,
            Estado = lockerExistente.Estado,
            Tamano = lockerExistente.Tamano
        };
    }
}