namespace GradeCom.Dtos.UserDtos;

public class AuthResponseDto
{
    // public required string token { get; set; }
    public required UserDto UserDto { get; set; }
    // public required IList<string> UserRole { get; set; }
}