using GradeCom.Dtos.UserDtos;
using GradeCom.Models;

namespace GradeCom.Utilits;

public static class UserMapper
{
    public static UserDto UserUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.UserName,
            Surname = user.Surname,
        };
    }

    public static User UserDtoUser(UserDto userDto)
    {
        return new User
        {
            Id = userDto.Id,
            Name = userDto.Name,
            Surname = userDto.Surname,
        };
    }

    public static void UserDtoUser(UserDto userDto, User user)
    {
        user.Name = userDto.Name;
        user.Surname = userDto.Surname;
    }
}