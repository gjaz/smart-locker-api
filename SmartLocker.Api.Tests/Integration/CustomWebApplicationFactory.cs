using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartLocker.Api.Data;

namespace SmartLocker.Api.Tests.Integration;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    public CustomWebApplicationFactory()
    {
        // Configuración JWT exclusiva para Integration Tests
        Environment.SetEnvironmentVariable(
            "Jwt__Key",
            "SmartLocker-Integration-Test-Key-12345678901234567890");

        Environment.SetEnvironmentVariable(
            "Jwt__Issuer",
            "SmartLocker.Api");

        Environment.SetEnvironmentVariable(
            "Jwt__Audience",
            "SmartLocker.Ui");

        // Usuario Admin exclusivo para Integration Tests
        Environment.SetEnvironmentVariable(
            "SeedAdmin__Username",
            "admin");

        Environment.SetEnvironmentVariable(
            "SeedAdmin__Password",
            "AdminTest123!");

        Environment.SetEnvironmentVariable(
            "SeedAdmin__Role",
            "Admin");

        // Usuario estándar exclusivo para Integration Tests
        Environment.SetEnvironmentVariable(
            "SeedUser__Username",
            "user");

        Environment.SetEnvironmentVariable(
            "SeedUser__Password",
            "UserTest123!");

        Environment.SetEnvironmentVariable(
            "SeedUser__Role",
            "User");
    }

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Eliminar la configuración de SQL Server registrada
            // originalmente por Program.cs.
            services.RemoveAll<
                DbContextOptions<SmartLockerDbContext>>();

            services.RemoveAll<DbContextOptions>();

            // Registrar una base InMemory exclusiva para tests.
            services.AddDbContext<SmartLockerDbContext>(options =>
            {
                options.UseInMemoryDatabase(
                    "SmartLockerIntegrationTests");
            });
        });
    }
}