using System.Collections;
using System.Net;
using GradeCom.Context;
using GradeCom.Dtos.Group;
using GradeCom.Dtos.Subject;
using GradeCom.Dtos.Teacher;
using GradeCom.Dtos.UserDtos;
using GradeCom.Enum;
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

    public async Task AddStudentToGroup(AssignStudentsToGroupDto groupDto)
    {
        var group = await _dbContext.Groups.FindAsync(groupDto.GroupId);
        if (group == null)
            throw new Exception("Group not found");
        
        var students = await _dbContext.Students
            .Where(s => groupDto.StudentIds.Contains(s.Id) && (s.GroupId == null || s.GroupId != groupDto.GroupId))
            .ToListAsync();

        foreach (var student in students)
        {
            student.GroupId = groupDto.GroupId;
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteStudentFromGroup(AssignStudentsToGroupDto groupDto)
    {
        var group = await _dbContext.Groups.FindAsync(groupDto.GroupId);
        if (group == null)
            throw new Exception("Group not found");
        
        var students = await _dbContext.Students
            .Where(s => groupDto.StudentIds.Contains(s.Id) && (s.GroupId == groupDto.GroupId))
            .ToListAsync();

        foreach (var student in students)
        {
            student.GroupId = null;
        }

        await _dbContext.SaveChangesAsync();    
    }

    public async Task CreateSubject(SubjectCreateDto subjectDto)
    {
        if (subjectDto is null)
            throw new HttpException(StatusCodes.Status400BadRequest,
                "You enter invalid subject");
        var subject = new Subject
        {
            Name = subjectDto.SubjectName
        };
        _dbContext.Subjects.Add(subject);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddTeacherToSubject(TeacherAddDto teacherDto)
    {
        if (teacherDto is null)
            throw new HttpException(StatusCodes.Status400BadRequest, "You enter invalid teacher");

        var teacher = await _dbContext.Teachers
            .Where(t => t.UserId == teacherDto.UserId)
            .FirstAsync();
        
        if (teacher == null)
            throw new HttpException(StatusCodes.Status404NotFound, "Teacher not found");
        var subject = await _dbContext.Subjects.FindAsync(teacherDto.SubjectId);
        if (subject is null)
            throw new HttpException(StatusCodes.Status400BadRequest, "Subject not found");
        switch (teacherDto.SubjectRole)
        {
            case SubjectRoleType.Lecturer:
                subject.LecturerTeacherId = teacher.Id;
                break;
            case SubjectRoleType.Practitioner:
                subject.PracticeTeacherId = teacher.Id;
                break;
        }
       
        await _dbContext.SaveChangesAsync();
        
    }

    public async Task AddGroupToSubject(GroupAddToSubjectDto toSubjectDto)
    {
        var subject = await _dbContext.Subjects
            .Include(s => s.Groups)
            .FirstOrDefaultAsync(s => s.Id == toSubjectDto.SubjectId);

        var group = await _dbContext.Groups.FindAsync(toSubjectDto.GroupId);

        if (group == null || subject == null)
            throw new Exception("Group or Subject not found");

        if (!subject.Groups.Contains(group))
            subject.Groups.Add(group);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteTeacherFromSubject(TeacherAddDto teacherDto)
    {
        if (teacherDto is null)
            throw new HttpException(StatusCodes.Status400BadRequest, "You enter invalid teacher");
        
        var teacher = await _dbContext.Teachers
            .Where(t => t.UserId == teacherDto.UserId)
            .FirstAsync();
        if (teacher == null)
            throw new HttpException(StatusCodes.Status404NotFound, "Teacher not found");
        var subject = await _dbContext.Subjects.FindAsync(teacherDto.SubjectId);
        if (subject is null)
            throw new HttpException(StatusCodes.Status400BadRequest, "Subject not found");
        switch (teacherDto.SubjectRole)
        {
            case SubjectRoleType.Lecturer:
                subject.LecturerTeacherId = null;
                break;
            case SubjectRoleType.Practitioner:
                subject.PracticeTeacherId = null;
                break;
        }
       
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteGroupFromSubject(GroupAddToSubjectDto toSubjectDto)
    {
        var subject = await _dbContext.Subjects
            .Include(s => s.Groups)
            .FirstOrDefaultAsync(s => s.Id == toSubjectDto.SubjectId);

        var group = await _dbContext.Groups.FindAsync(toSubjectDto.GroupId);

        if (group == null || subject == null)
            throw new Exception("Group or Subject not found");

        if (!subject.Groups.Contains(group))
            subject.Groups.Remove(group);
        
        await _dbContext.SaveChangesAsync();
    }

    public Task DeleteSubject(int subjectId)
    {
        var subject = _dbContext.Subjects.Find(subjectId);
        if (subject == null)
            throw new Exception("Subject not found");
        _dbContext.Subjects.Remove(subject);
        return _dbContext.SaveChangesAsync();
    }

    public Task DeleteGroup(int groupId)
    {
        var group = _dbContext.Groups.Find(groupId);
        if (group == null)
            throw new Exception("Group not found");
        _dbContext.Groups.Remove(group);
        return _dbContext.SaveChangesAsync();
    }

    public async Task<List<SubjectShowDto>> GetAllSubjects()
    {
        var subjects = await _dbContext.Subjects
            .Include(s => s.LecturerTeacher)
            .ThenInclude(t => t.User)
            .Include(s => s.PracticeTeacher)
            .ThenInclude(t => t.User)
            .Include(s => s.Groups) 
            .ToListAsync();

        return subjects.Select(s => new SubjectShowDto
        {
            Id = s.Id,
            Name = s.Name,
            LecturerTeacher = s.LecturerTeacher == null ? null : new TeacherDto
            {
                Id = s.LecturerTeacher.Id,
                FirstName = s.LecturerTeacher.User.UserName,
                LastName = s.LecturerTeacher.User.Surname
            },
            PracticeTeacher = s.PracticeTeacher == null ? null : new TeacherDto
            {
                Id = s.PracticeTeacher.Id,
                FirstName = s.PracticeTeacher.User.UserName,
                LastName = s.PracticeTeacher.User.Surname
            },
            Groups = s.Groups?.Select(g => new GroupCreateDto
            {
                Name = g.Name
            }).ToList() ?? new List<GroupCreateDto>()
        }).ToList();
    }

    public async Task<List<User>> GetAllTeachers()
    {
        var teachers =  await _dbContext.Teachers
            .ToListAsync();
        List<User> teacherUsers = new List<User>();
        foreach (var teacher in teachers)
        {
            teacherUsers.Add(await _dbContext.Users.FindAsync(teacher.UserId));
        }
        return teacherUsers;
    }

    public async Task<List<Group>> GetAllGroups()
    {
        return await _dbContext.Groups
            .ToListAsync();    
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