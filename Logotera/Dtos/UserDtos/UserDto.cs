using System.Text.Json.Serialization;

namespace Logotera.Dtos.UserDtos;

public class UserDto
{
    public string? Id { get; init; }
    public string? UserName { get; init; }
    public string? Surname { get; init; }
    public string? Email { get; init; }
    public string? Description { get; set; }
    public IFormFile? ProfileImageFile { get; set; } 
  
}