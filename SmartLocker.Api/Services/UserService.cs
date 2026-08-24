using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartLocker.Api.Data;
using SmartLocker.Api.Models;

namespace SmartLocker.Api.Services;

public class UserService : IUserService
{
    private readonly SmartLockerDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(SmartLockerDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task<User> CreateAsync(
        string username,
        string password,
        string role)
    {
        var user = new User
        {
            Username = username,
            Role = role
        };

        user.PasswordHash =
            _passwordHasher.HashPassword(user, password);

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> ValidateCredentialsAsync(
    string username,
    string password)
    {
        var user = await GetByUsernameAsync(username);

        if (user == null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            password);

        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return user;
    }
}
