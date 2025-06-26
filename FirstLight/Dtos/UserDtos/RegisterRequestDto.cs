using System.ComponentModel.DataAnnotations;

namespace FirstLight.Dtos.UserDtos;

public class RegisterRequestDto
{
    [Required] 
    public string? Name { get; set; }
    [Required]
    public string? Surname { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
    [Required]
    public string? Role { get; set; }
}