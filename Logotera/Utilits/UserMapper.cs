using Logotera.Dtos.UserDtos;
using Logotera.Models;

namespace Logotera.Utilits;

public static class UserMapper
{
    public static UserDto UserUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Surname = user.Surname,
            Email = user.Email,
            Description = user.Description,
        };
    }
    
    public static async Task<User> UserDtoToUserAsync(UserDto dto)
    {
        var user = new User
        {
            UserName = dto.UserName,
            Surname = dto.Surname,
            Email = dto.Email,
            Description = dto.Description
        };

        if (dto.ProfileImageFile != null)
        {
            user.ProfileImagePath = await UserImageUpdate(dto.ProfileImageFile);
        }

        return user;
    }

    public static User UserDtoUser(UserDto userDto)
    {
        return new User
        {
            Id = userDto.Id,
            UserName = userDto.UserName,
            Surname = userDto.Surname,
            Email = userDto.Email,
            Description = userDto.Description
        };
    }

    public static void UserDtoUser(UserDto userDto, User user)
    {
        user.UserName = userDto.UserName;
        user.Surname = userDto.Surname;
        user.Email = userDto.Email;
        user.Description = userDto.Description;
        // user.ProfileImagePath = UserImageUpdate(userDto);
    }

    public static void UserDtoUser(User user, string? description)
    {
        user.Description = description;
    }

    public static async Task<string?> UserImageUpdate(IFormFile? file)
    {
        if (file == null) return null;

        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/images/{fileName}"; 
    }
    
    
}