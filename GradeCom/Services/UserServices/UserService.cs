using System.Net;
using GradeCom.Context;
using GradeCom.Dtos.UserDtos;
using GradeCom.Exceptions;
using GradeCom.Utilits;
using Microsoft.EntityFrameworkCore;

namespace GradeCom.Services.UserServices;

public class UserService : IUserService
{
    private readonly GrateContext _dbContext;

    public UserService(GrateContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<UserDto> Post(UserDto userDto)
    {
        if (userDto is null)
            throw new HttpException(StatusCodes.Status400BadRequest,
                "Вы неверно ввели данные");

        var user = UserMapper.UserDtoUser(userDto);

        if (user.UserName is null ||
            user.Surname is null ||
            user.Email is null)
            throw new HttpException(StatusCodes.Status400BadRequest,
                "Вы ввели данные неверно");

        if (_dbContext.Users.Any(u => u.UserName == userDto.UserName
                                      || u.Surname == userDto.Surname
                                      || u.Email == userDto.Email))
            throw new HttpException(StatusCodes.Status400BadRequest,
                "Пользователь с такими данными уже существует");

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return UserMapper.UserUserDto(user);
    }

    // public async Task<IEnumerable<UserDto>> Get(string userId)
    // {
    //     var users = await _dbContext.Users
    //         .ToListAsync();
    //     
    //     return users.Select(UserMapper.UserUserDto);
    // }

    public async Task<UserDto> Get(string id)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (user is null)
            throw new HttpException(StatusCodes.Status404NotFound,
                "Пользователь с таким id не был найдке");
        
        return UserMapper.UserUserDto(user);
    }

    public async Task<UserDto> Put(UserDto userDto)
    {
        if (userDto is null)
            throw new HttpException(StatusCodes.Status400BadRequest,
                "Вы ввели данные неверно");
        
        var user = UserMapper.UserDtoUser(userDto);

        if (await _dbContext.Users.AnyAsync(u => u.Id != user.Id &&
                                                 u.Email == user.Email))
            throw new HttpException(HttpStatusCode.BadRequest,
                $"Пользователь с email:{userDto.Email} уже существует"); 
        
        user = await _dbContext.Users.FindAsync(userDto.Id);

        if (user is null)
            throw new HttpException(StatusCodes.Status400BadRequest,
                "Вы ввели неверно данные");

        UserMapper.UserDtoUser(userDto, user);
        
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        return UserMapper.UserUserDto(user);
    }

    public async Task<UserDto> Patch(string id, string description)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
            throw new HttpException(StatusCodes.Status400BadRequest,
                "Вы ввели id неверно");
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
                "Вы ввели id неверно");

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
}