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
            UserName = user.UserName,
            Surname = user.Surname,
            Email = user.Email
        };
    }

    public static User UserDtoUser(UserDto userDto)
    {
        return new User
        {
            Id = userDto.Id,
            UserName = userDto.UserName,
            Surname = userDto.Surname,
            Email = userDto.Email
        };
    }

    public static void UserDtoUser(UserDto userDto, User user)
    {
        user.UserName = userDto.UserName;
        user.Surname = userDto.Surname;
        user.Email = userDto.Email;
    }
}