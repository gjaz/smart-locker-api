using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartLocker.Api.DTOs;
using SmartLocker.Api.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartLocker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IConfiguration configuration,
        IUserService userService,
        ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        _logger.LogInformation(
            "Login attempt for user {Username}",
            dto.Username);

        var user = await _userService.ValidateCredentialsAsync(
            dto.Username,
            dto.Password);

        if (user == null)
        {
            _logger.LogWarning(
                "Login failed for user {Username}",
                dto.Username);

            return Unauthorized();
        }

        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT Key no configurada.");

        var jwtIssuer = _configuration["Jwt:Issuer"];
        var jwtAudience = _configuration["Jwt:Audience"];

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: credentials);

        var tokenString =
            new JwtSecurityTokenHandler().WriteToken(token);

        _logger.LogInformation(
            "Login successful for user {Username} with role {Role}",
            user.Username,
            user.Role);

        return Ok(new
        {
            token = tokenString
        });
    }
}