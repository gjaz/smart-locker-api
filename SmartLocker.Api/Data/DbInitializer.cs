using SmartLocker.Api.Services;

namespace SmartLocker.Api.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var userService =
            scope.ServiceProvider.GetRequiredService<IUserService>();

        var admin = await userService.GetByUsernameAsync("admin");

        if (admin is null)
        {
            await userService.CreateAsync(
                "admin",
                "1234",
                "Admin");
        }
    }
}