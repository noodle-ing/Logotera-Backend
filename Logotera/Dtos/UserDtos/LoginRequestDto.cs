using System.ComponentModel.DataAnnotations;

namespace Logotera.Dtos.UserDtos;

public class LoginRequestDto
{
    [Required]
    public string? Email { get; init; }
    [Required]
    public string? Password { get; init; }
}