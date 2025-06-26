using System.Collections;
using System.Net;
using Logotera.Context;
using Logotera.Dtos.UserDtos;
using Logotera.Exceptions;
using Logotera.Models;
using Logotera.Utilits;
using Microsoft.EntityFrameworkCore;

namespace Logotera.Services.UserServices;

public class UserService : IUserService
{
    private readonly GrateContext _dbContext;
    private readonly IWebHostEnvironment _env;

    public UserService(IWebHostEnvironment env, GrateContext dbContext)
    {
        _dbContext = dbContext;
        _env = env;
    }
    
    public async Task<UserDto> Post(UserDto userDto)
    {
        if (userDto is null)
            throw new HttpException(StatusCodes.Status400BadRequest,
                "You entered wrong parameters!");

        var user = await UserMapper.UserDtoToUserAsync(userDto);

        if (user.UserName is null ||
            user.Surname is null ||
            user.Email is null)
            throw new HttpException(StatusCodes.Status400BadRequest,
                "You entered wrong parameters!");

        if (_dbContext.Users.Any(u => u.UserName == userDto.UserName
                                      || u.Surname == userDto.Surname
                                      || u.Email == userDto.Email))
            throw new HttpException(StatusCodes.Status400BadRequest,
                "User with such parameters doesn't exist!");

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return UserMapper.UserUserDto(user);
    }
    
    public async Task<UserDto> Get(string email)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        
        if (user is null)
            throw new HttpException(StatusCodes.Status404NotFound,
                "User with such email doesn't found");
        
        return UserMapper.UserUserDto(user);
    }

    public async Task<UserDto> Put(UserDto userDto)
    {
        if (userDto is null)
            throw new HttpException(StatusCodes.Status400BadRequest,
                "You entered wrong parameters!");
        
        var user = UserMapper.UserDtoUser(userDto);

        if (await _dbContext.Users.AnyAsync(u => u.Id != user.Id &&
                                                 u.Email == user.Email))
            throw new HttpException(HttpStatusCode.BadRequest,
                $"User with such email:{userDto.Email} already exists!"); 
        
        user = await _dbContext.Users.FindAsync(userDto.Id);

        if (user is null)
            throw new HttpException(StatusCodes.Status400BadRequest,
                "You entered wrong parameters!");

        UserMapper.UserDtoUser(userDto, user);
        
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        return UserMapper.UserUserDto(user);
    }

    public async Task<UserDto> Patch(string id, string? description)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
            throw new HttpException(StatusCodes.Status400BadRequest,
                "You entered id incorrect");
        UserMapper.UserDtoUser(user, description);
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        return UserMapper.UserUserDto(user);
    }

    public async Task Delete(string id)
    {
        var user = await _dbContext.Users.FindAsync(id);

        if (user is null)
            throw new HttpException(StatusCodes.Status400BadRequest,
                "You entered id incorrect");

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<User>> GetAllUsersAsync(string email)
    {
        return await _dbContext.Users
            .Where(u => u.Email != email)
            .ToListAsync();
    }
}