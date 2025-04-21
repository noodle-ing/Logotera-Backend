using GradeCom.Dtos.UserDtos;

namespace GradeCom.Services.UserServices;

public interface IUserService
{
    Task<UserDto> Get(string id);
    Task<UserDto> Post(UserDto userDto);
    Task<UserDto> Put(UserDto userDto);
    Task<UserDto> Patch(string id, string description);
    Task Delete(string id);
}