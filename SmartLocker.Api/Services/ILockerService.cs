using SmartLocker.Api.Models;

namespace SmartLocker.Api.Services;

public interface ILockerService
{
    Task<IEnumerable<Locker>> GetAllAsync();

    Task<Locker> AddAsync(Locker locker);

    Task<bool> DeleteAsync(int id);
}