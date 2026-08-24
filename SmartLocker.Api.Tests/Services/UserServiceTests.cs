using Microsoft.EntityFrameworkCore;
using SmartLocker.Api.Data;
using SmartLocker.Api.Models;
using SmartLocker.Api.Services;

namespace SmartLocker.Api.Tests.Services;

public class UserServiceTests
{
    private static SmartLockerDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SmartLockerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SmartLockerDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_Should_Create_User_With_Hashed_Password()
    {
        // Arrange
        using var context = CreateContext();

        var service = new UserService(context);

        const string username = "admin";
        const string password = "1234";
        const string role = "Admin";

        // Act
        var result = await service.CreateAsync(
            username,
            password,
            role);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(username, result.Username);
        Assert.Equal(role, result.Role);

        Assert.NotEqual(password, result.PasswordHash);
        Assert.False(string.IsNullOrWhiteSpace(result.PasswordHash));

        Assert.Single(context.Users);
    }

    [Fact]
    public async Task GetByUsernameAsync_Should_Return_Existing_User()
    {
        // Arrange
        using var context = CreateContext();

        var user = new User
        {
            Username = "user",
            PasswordHash = "hash-prueba",
            Role = "User"
        };

        context.Users.Add(user);

        await context.SaveChangesAsync();

        var service = new UserService(context);

        // Act
        var result = await service.GetByUsernameAsync("user");

        // Assert
        Assert.NotNull(result);

        Assert.Equal("user", result.Username);
        Assert.Equal("User", result.Role);
    }

    [Fact]
    public async Task ValidateCredentialsAsync_Should_Return_User_When_Password_Is_Correct()
    {
        // Arrange
        using var context = CreateContext();

        var service = new UserService(context);

        await service.CreateAsync(
            "admin",
            "1234",
            "Admin");

        // Act
        var result = await service.ValidateCredentialsAsync(
            "admin",
            "1234");

        // Assert
        Assert.NotNull(result);

        Assert.Equal("admin", result.Username);
        Assert.Equal("Admin", result.Role);
    }

    [Fact]
    public async Task ValidateCredentialsAsync_Should_Return_Null_When_Password_Is_Incorrect()
    {
        // Arrange
        using var context = CreateContext();

        var service = new UserService(context);

        await service.CreateAsync(
            "admin",
            "1234",
            "Admin");

        // Act
        var result = await service.ValidateCredentialsAsync(
            "admin",
            "incorrecta");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ValidateCredentialsAsync_Should_Return_Null_When_User_Does_Not_Exist()
    {
        // Arrange
        using var context = CreateContext();

        var service = new UserService(context);

        // Act
        var result = await service.ValidateCredentialsAsync(
            "usuario-inexistente",
            "1234");

        // Assert
        Assert.Null(result);
    }
}
