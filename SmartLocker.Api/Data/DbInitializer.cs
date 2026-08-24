using SmartLocker.Api.Services;

namespace SmartLocker.Api.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        using var scope = services.CreateScope();

        var userService =
            scope.ServiceProvider.GetRequiredService<IUserService>();

        var username = configuration["SeedAdmin:Username"]
            ?? throw new InvalidOperationException(
                "SeedAdmin Username no configurado.");

        var password = configuration["SeedAdmin:Password"]
            ?? throw new InvalidOperationException(
                "SeedAdmin Password no configurado.");

        var role = configuration["SeedAdmin:Role"]
            ?? throw new InvalidOperationException(
                "SeedAdmin Role no configurado.");

        var admin = await userService.GetByUsernameAsync(username);

        if (admin is null)
        {
            await userService.CreateAsync(
                username,
                password,
                role);
        }
    }
}