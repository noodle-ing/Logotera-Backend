using System.ComponentModel.DataAnnotations;

namespace FirstLight.Dtos.UserDtos;

public class LoginRequestDto
{
    [Required]
    public string? Email { get; init; }
    [Required]
    public string? Password { get; init; }
}