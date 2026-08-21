using Microsoft.EntityFrameworkCore;
using SmartLocker.Api.Data;
using SmartLocker.Api.Models;

namespace SmartLocker.Api.Services;

public class LockerService : ILockerService
{
    private readonly SmartLockerDbContext _context;

    public LockerService(SmartLockerDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Locker>> GetAllAsync()
    {
        return await _context.Lockers.ToListAsync();
    }

    public async Task<Locker> AddAsync(Locker locker)
    {
        _context.Lockers.Add(locker);

        await _context.SaveChangesAsync();

        return locker;
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
}