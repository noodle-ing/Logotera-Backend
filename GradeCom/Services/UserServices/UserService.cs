using System.Collections;
using System.Net;
using GradeCom.Context;
using GradeCom.Dtos.Group;
using GradeCom.Dtos.UserDtos;
using GradeCom.Exceptions;
using GradeCom.Models;
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

        var user = await UserMapper.UserDtoToUserAsync(userDto);

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

    public async Task<UserDto> Get(string email)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        
        if (user is null)
            throw new HttpException(StatusCodes.Status404NotFound,
                "Пользователь с таким email не был найдке");
        
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

    public async Task<UserDto> Patch(string id, string? description)
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
    
    
    public async Task CreateGroup(GroupCreateDto groupDto)
    {
        if (groupDto is null)
            throw new HttpException(StatusCodes.Status400BadRequest,
                "Вы неверно ввели данные");
        var group = new Group
        {
            Name = groupDto.Name
        };
        _dbContext.Groups.Add(group);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddStudentToGroup(AssignStudentsToGroupDto dto)
    {
        var group = await _dbContext.Groups.FindAsync(dto.GroupId);
        if (group == null)
            throw new Exception("Group not found");
        
        var students = await _dbContext.Students
            .Where(s => dto.StudentIds.Contains(s.Id) && (s.GroupId == null || s.GroupId != dto.GroupId))
            .ToListAsync();

        foreach (var student in students)
        {
            student.GroupId = dto.GroupId;
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteStudentFromGroup(AssignStudentsToGroupDto dto)
    {
        var group = await _dbContext.Groups.FindAsync(dto.GroupId);
        if (group == null)
            throw new Exception("Group not found");
        
        var students = await _dbContext.Students
            .Where(s => dto.StudentIds.Contains(s.Id) && (s.GroupId == dto.GroupId))
            .ToListAsync();

        foreach (var student in students)
        {
            student.GroupId = null;
        }

        await _dbContext.SaveChangesAsync();    
    }


    public async Task Put(string id, IFormFile file)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
            throw new HttpException(StatusCodes.Status400BadRequest,
                "Вы ввели id неверно");

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine("wwwroot/images", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        user.ProfileImagePath = $"/images/{fileName}";
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<User>> GetAllUsersAsync(string email)
    {
        return await _dbContext.Users
            .Where(u => u.Email != email)
            .ToListAsync();
    }
}