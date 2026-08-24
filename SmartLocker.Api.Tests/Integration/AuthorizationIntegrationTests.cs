using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SmartLocker.Api.Tests.Integration;

public class AuthorizationIntegrationTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthorizationIntegrationTests(
        CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Lockers_Without_Token_Should_Return_Unauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/lockers");

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task Get_Lockers_With_User_Token_Should_Return_Ok()
    {
        // Arrange
        var token = await LoginAsync(
            "user",
            "UserTest123!");

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        // Act
        var response = await _client.GetAsync("/api/lockers");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Post_Locker_With_User_Token_Should_Return_Forbidden()
    {
        // Arrange
        var token = await LoginAsync(
            "user",
            "UserTest123!");

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        var locker = new
        {
            codigo = "L-INT-001",
            ubicacion = "Integracion",
            estado = "Disponible",
            tamano = "Mediano"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/lockers",
            locker);

        // Assert
        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    [Fact]
    public async Task Post_Locker_With_Admin_Token_Should_Return_Ok()
    {
        // Arrange
        var token = await LoginAsync(
            "admin",
            "AdminTest123!");

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        var locker = new
        {
            codigo = "L-INT-002",
            ubicacion = "Integracion",
            estado = "Disponible",
            tamano = "Grande"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/lockers",
            locker);

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    private async Task<string> LoginAsync(
        string username,
        string password)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new
            {
                username,
                password
            });

        response.EnsureSuccessStatusCode();

        var result =
            await response.Content.ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));

        return result.Token;
    }

    private sealed class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
