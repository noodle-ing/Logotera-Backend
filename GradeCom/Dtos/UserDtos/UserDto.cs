namespace GradeCom.Dtos.UserDtos;

public class UserDto
{
    public string? Id { get; init; }
    public string? UserName { get; init; }
    public string? Surname { get; init; }
    public string? Email { get; init; }
    public string? Descripton { get; set; }
}