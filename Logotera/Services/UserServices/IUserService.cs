using Logotera.Dtos.UserDtos;
using Logotera.Models;

namespace Logotera.Services.UserServices;

public interface IUserService
{
    Task<UserDto> Get(string id);
    Task<UserDto> Post(UserDto userDto);
    Task<UserDto> Put(UserDto userDto);
    Task<UserDto> Patch(string id, string description);
    Task Delete(string id);
    Task<List<User>> GetAllUsersAsync(string userId);
}