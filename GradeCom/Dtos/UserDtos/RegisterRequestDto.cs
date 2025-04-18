using System.ComponentModel.DataAnnotations;

namespace GradeCom.Dtos.UserDtos;

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
}