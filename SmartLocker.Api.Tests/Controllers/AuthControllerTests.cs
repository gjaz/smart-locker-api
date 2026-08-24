using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SmartLocker.Api.Controllers;
using SmartLocker.Api.DTOs;
using SmartLocker.Api.Models;
using SmartLocker.Api.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmartLocker.Api.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly IConfiguration _configuration;

    public AuthControllerTests()
    {
        _mockUserService = new Mock<IUserService>();

        var settings = new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "SmartLocker-Test-Key-12345678901234567890",
            ["Jwt:Issuer"] = "SmartLocker.Api",
            ["Jwt:Audience"] = "SmartLocker.Ui"
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }

    [Fact]
    public async Task Login_Should_Return_Ok_With_Token_When_Credentials_Are_Valid()
    {
        // Arrange
        var dto = new LoginDto
        {
            Username = "admin",
            Password = "1234"
        };

        var user = new User
        {
            Id = 1,
            Username = "admin",
            Role = "Admin",
            PasswordHash = "hash"
        };

        _mockUserService
            .Setup(x => x.ValidateCredentialsAsync(
                dto.Username,
                dto.Password))
            .ReturnsAsync(user);

        var controller = new AuthController(
            _configuration,
            _mockUserService.Object);

        // Act
        var result = await controller.Login(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);

        _mockUserService.Verify(
            x => x.ValidateCredentialsAsync(
                dto.Username,
                dto.Password),
            Times.Once);
    }

    [Fact]
    public async Task Login_Should_Return_Unauthorized_When_Credentials_Are_Invalid()
    {
        // Arrange
        var dto = new LoginDto
        {
            Username = "admin",
            Password = "incorrecta"
        };

        _mockUserService
            .Setup(x => x.ValidateCredentialsAsync(
                dto.Username,
                dto.Password))
            .ReturnsAsync((User?)null);

        var controller = new AuthController(
            _configuration,
            _mockUserService.Object);

        // Act
        var result = await controller.Login(dto);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);

        _mockUserService.Verify(
            x => x.ValidateCredentialsAsync(
                dto.Username,
                dto.Password),
            Times.Once);
    }

    [Fact]
    public async Task Login_Should_Generate_Token_With_Correct_Name_And_Role()
    {
        // Arrange
        var dto = new LoginDto
        {
            Username = "admin",
            Password = "1234"
        };

        var user = new User
        {
            Id = 1,
            Username = "admin",
            Role = "Admin",
            PasswordHash = "hash"
        };

        _mockUserService
            .Setup(x => x.ValidateCredentialsAsync(
                dto.Username,
                dto.Password))
            .ReturnsAsync(user);

        var controller = new AuthController(
            _configuration,
            _mockUserService.Object);

        // Act
        var result = await controller.Login(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        var tokenProperty = okResult.Value!
            .GetType()
            .GetProperty("token");

        Assert.NotNull(tokenProperty);

        var tokenString =
            tokenProperty.GetValue(okResult.Value)?.ToString();

        Assert.False(string.IsNullOrWhiteSpace(tokenString));

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(tokenString);

        var nameClaim = jwt.Claims
            .First(x => x.Type == ClaimTypes.Name);

        var roleClaim = jwt.Claims
            .First(x => x.Type == ClaimTypes.Role);

        Assert.Equal("admin", nameClaim.Value);
        Assert.Equal("Admin", roleClaim.Value);
    }
}