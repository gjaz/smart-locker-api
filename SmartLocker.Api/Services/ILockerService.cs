using SmartLocker.Api.Models;

namespace SmartLocker.Api.Services;

public interface ILockerService
{
    IEnumerable<Locker> GetAll();
    Locker Add(Locker locker);
    bool Delete(int id);
}
