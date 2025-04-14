using GradeCom.Dtos.UserDtos;

namespace GradeCom.Services.UserServices;

public interface IUserService
{
    Task<UserDto> Get(string id);
    Task<UserDto> Post(UserDto userDto);
    Task<UserDto> Put(UserDto userDto);
    Task Delete(string id);
}