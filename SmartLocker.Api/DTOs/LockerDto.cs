namespace SmartLocker.Api.DTOs;

public class LockerDto
{
    public int Id { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Ubicacion { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public string Tamano { get; set; } = string.Empty;
}