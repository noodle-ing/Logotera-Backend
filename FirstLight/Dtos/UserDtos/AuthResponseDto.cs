namespace FirstLight.Dtos.UserDtos;

public class AuthResponseDto
{
    public required string token { get; set; }
    public required UserDto UserDto { get; set; }
    public string UserRole { get; set; } 
}