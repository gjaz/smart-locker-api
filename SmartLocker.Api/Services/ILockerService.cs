using SmartLocker.Api.Models;
using SmartLocker.Api.DTOs;

namespace SmartLocker.Api.Services;

public interface ILockerService
{
    Task<IEnumerable<LockerDto>> GetAllAsync();

    Task<LockerDto> AddAsync(CreateLockerDto dto);

    Task<bool> DeleteAsync(int id);

    Task<LockerDto?> UpdateAsync(int id, UpdateLockerDto dto);
}