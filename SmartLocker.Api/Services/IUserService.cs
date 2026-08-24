using SmartLocker.Api.Models;

namespace SmartLocker.Api.Services;

public interface IUserService
{
    Task<User?> GetByUsernameAsync(string username);

    Task<User> CreateAsync(
        string username,
        string password,
        string role);

    Task<User?> ValidateCredentialsAsync(
    string username,
    string password);
}