using System.ComponentModel.DataAnnotations;

namespace SmartLocker.Api.DTOs;

public class UpdateLockerDto
{
    [Required]
    [MinLength(3)]
    public string Codigo { get; set; } = string.Empty;

    [Required]
    public string Ubicacion { get; set; } = string.Empty;

    [Required]
    public string Estado { get; set; } = string.Empty;

    [Required]
    public string Tamano { get; set; } = string.Empty;
}
