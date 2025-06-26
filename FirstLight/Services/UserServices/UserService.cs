using System.Collections;
using System.Net;
using FirstLight.Context;
using FirstLight.Dtos.File;
using FirstLight.Dtos.Group;
using FirstLight.Dtos.ModuleDto;
using FirstLight.Dtos.StudentDto;
using FirstLight.Dtos.Subject;
using FirstLight.Dtos.Teacher;
using FirstLight.Dtos.UserDtos;
using FirstLight.Enum;
using FirstLight.Exceptions;
using FirstLight.Models;
using FirstLight.Models.Files;
using FirstLight.Models.Files.Interface;
using FirstLight.Utilits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstLight.Services.UserServices;

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
    
    
    public async Task CreateGroup(GroupCreateDto groupDto)
    {
        if (groupDto is null)
            throw new HttpException(StatusCodes.Status400BadRequest,
                "You entered wrong parameters!");
        var group = new Group
        {
            Name = groupDto.Name
        };
        _dbContext.Groups.Add(group);
        await _dbContext.SaveChangesAsync();
    }
    
    public Task DeleteGroup(int groupId)
    {
        var group = _dbContext.Groups.Find(groupId);
        if (group == null)
            throw new Exception("Group not found");
        _dbContext.Groups.Remove(group);
        return _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<GroupWithStudentDto>> GetAllGroups()
    {
        var groups = await _dbContext.Groups
            .Include(g => g.Students)
            .ThenInclude(s => s.User)
            .ToListAsync();

        var result = groups.Select(g => new GroupWithStudentDto
        {
            Id = g.Id,
            Name = g.Name,
            Students = g.Students != null
                ? g.Students.Select(s => new StudentUserDto
                {
                    StudentId = s.Id,
                    Name = s.User.UserName,  
                    Surname = s.User.Surname
                }).ToList()
                : new List<StudentUserDto>()
        }).ToList();

        return result; 
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

    public async Task DeleteTeacherFromSubject(TeacherDeleteFromSubjectDto teacherDto)
    {
        if (teacherDto is null)
            throw new HttpException(StatusCodes.Status400BadRequest, "You enter invalid teacher");
        
        var teacher = await _dbContext.Teachers
            .Where(t =>  t.Id == teacherDto.TeacherId)
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
            Groups = s.Groups?.Select(g => new GroupViewDto
            {
                Id = g.Id,
                Name = g.Name
            }).ToList() ?? new List<GroupViewDto>()
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

    public async Task<List<StudentUserDto>> GetAllStudent()
    {
        var students = await _dbContext.Students
            .Include(s => s.User)
            .ToListAsync();

        var result = students.Select(s => new StudentUserDto
        {
            StudentId = s.Id,
            Name = s.User.UserName,    
            Surname = s.User.Surname
        }).ToList();

        return result;
    }
    
    public async Task<List<Subject>> GetAllTeacherSubject(string email)
    {
        User userTeacher = await _dbContext.Users
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();
        Teacher teacher  =await _dbContext.Teachers
            .Where(t => t.UserId == userTeacher.Id)
            .FirstOrDefaultAsync();
        if (teacher == null)
            throw new Exception("Teacher not found");
        var subjects = await _dbContext.Subjects
            .Where(s => s.PracticeTeacherId == teacher.Id || s.LecturerTeacherId == teacher.Id)
            .ToListAsync();
        return subjects;
    }
    
    public async Task<SubjectShowDto> GetSubjectForTeacher(int subjectId)
    {
        var subject = await _dbContext.Subjects
            .Include(s => s.LecturerTeacher).ThenInclude(t => t.User)
            .Include(s => s.PracticeTeacher).ThenInclude(t => t.User)
            .Include(s => s.Groups)
            .Include(s => s.Modules)
            .FirstOrDefaultAsync(s => s.Id == subjectId);

        if (subject == null)
            throw new Exception("Subject not found");

        var dto = new SubjectShowDto
        {
            Id = subject.Id,
            Name = subject.Name,
            SyllabusFilePath = subject.SyllabusFilePath,
            LecturerTeacher = subject.LecturerTeacher == null ? null : new TeacherDto
            {
                Id = subject.LecturerTeacher.Id,
                FirstName = subject.LecturerTeacher.User.UserName,
                LastName = subject.LecturerTeacher.User.Surname
            },
            PracticeTeacher = subject.PracticeTeacher == null ? null : new TeacherDto
            {
                Id = subject.PracticeTeacher.Id,
                FirstName = subject.PracticeTeacher.User.UserName,
                LastName = subject.PracticeTeacher.User.Surname
            },
            Groups = subject.Groups?.Select(g => new GroupViewDto
            {
                Id = g.Id,
                Name = g.Name
            }).ToList() ?? new List<GroupViewDto>(),

            Modules = new List<ModuleViewDto>()
        };

        foreach (var module in subject.Modules ?? new List<Module>())
        {
            var moduleDto = new ModuleViewDto
            {
                Id = module.Id,
                Title = module.Title,
                Description = module.Description
            };

            var lectureFiles = await _dbContext.LectureFiles.Where(f => f.ModuleId == module.Id).ToListAsync();
            var practiceFiles = await _dbContext.PracticeFiles.Where(f => f.ModuleId == module.Id).ToListAsync();
            var seminarFiles = await _dbContext.SeminarFiles.Where(f => f.ModuleId == module.Id).ToListAsync();
            var homeTaskFiles = await _dbContext.HomeTaskFiles.Where(f => f.ModuleId == module.Id).ToListAsync();

            foreach (var file in lectureFiles)
                moduleDto.LectureMaterial.Add(await GetMaterial(file.Id, "lecture"));

            foreach (var file in practiceFiles)
                moduleDto.PracticeMaterial.Add(await GetMaterial(file.Id, "practice"));

            foreach (var file in seminarFiles)
                moduleDto.SeminarMaterial.Add(await GetMaterial(file.Id, "seminar"));

            foreach (var file in homeTaskFiles)
                moduleDto.HomeMaterial.Add(await GetMaterial(file.Id, "homework"));

            dto.Modules.Add(moduleDto);
        }

        return dto;
    }


    public async Task AddSyllabus(int subjectId, IFormFile file)
    {
        var subject = await _dbContext.Subjects.FindAsync(subjectId);
        if (subject == null)
            throw new Exception("Subject not found");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedSyllabi");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        subject.SyllabusFilePath = filePath;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<FileDownloadInfo> GetSyllabus(int subjectId)
    {
        var subject = await _dbContext.Subjects.FindAsync(subjectId);
        if (subject == null || string.IsNullOrEmpty(subject.SyllabusFilePath))
            throw new Exception("Syllabus not found.");

        var filePath = subject.SyllabusFilePath;
        if (!System.IO.File.Exists(filePath))
            throw new Exception("Syllabus file does not exist on server.");

        return new FileDownloadInfo
        {
            FilePath = filePath,
            ContentType = "application/pdf", 
            FileName = Path.GetFileName(filePath)
        };
    }
    
    public async Task CreateModule(CreateModuleDto moduleDto)
    {
        var subject = await _dbContext.Subjects.FindAsync(moduleDto.SubjectId);
        if (subject == null)
            throw new Exception("Subject not found.");

        var module = new Module
        {
            Title = moduleDto.Title,
            Description = moduleDto.Description,
            SubjectId = moduleDto.SubjectId,
        };

        await _dbContext.Modules.AddAsync(module);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteModule(int moduleId)
    {
        Module module = await _dbContext.Modules.FindAsync(moduleId);
        _dbContext.Modules.Remove(module);
        await _dbContext.SaveChangesAsync();
    }

    public async Task EditModule(ModuleViewDto moduleDto)
    {
        Module module = await _dbContext.Modules.FindAsync(moduleDto.Id);
        module.Title = moduleDto.Title;
        module.Description = moduleDto.Description;
        await _dbContext.SaveChangesAsync();
    }

    public async Task UploadMaterialFile(int moduleId, string type, IFormFile file)
    {
        var uploadsPath = Path.Combine(_env.WebRootPath, "materials", type.ToLower());
        Directory.CreateDirectory(uploadsPath);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsPath, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var virtualPath = $"/materials/{type.ToLower()}/{fileName}";
        var contentType = file.ContentType;

        switch (type.ToLower())
        {
            case "lecture":
                _dbContext.LectureFiles.Add(new LectureFile
                {
                    FileName = file.FileName,
                    FilePath = virtualPath,
                    ContentType = contentType,
                    ModuleId = moduleId
                });
                break;
            case "practice":
                _dbContext.PracticeFiles.Add(new PracticeFile
                {
                    FileName = file.FileName,
                    FilePath = virtualPath,
                    ContentType = contentType,
                    ModuleId = moduleId
                });
                break;
            case "seminar":
                _dbContext.SeminarFiles.Add(new SeminarFile
                {
                    FileName = file.FileName,
                    FilePath = virtualPath,
                    ContentType = contentType,
                    ModuleId = moduleId
                });
                break;
            case "homework":
                _dbContext.HomeTaskFiles.Add(new HomeTaskFile
                {
                    FileName = file.FileName,
                    FilePath = virtualPath,
                    ContentType = contentType,
                    ModuleId = moduleId
                });
                break;
            default:
                throw new Exception("Invalid type.");
        }

        await _dbContext.SaveChangesAsync();
    }
    
    
    public async Task<FileDownloadInfo> GetMaterial(int fileId, string fileType)
    {
        IFileEntity? file = fileType.ToLower() switch
        {
            "lecture" => await _dbContext.LectureFiles.FirstOrDefaultAsync(f => f.Id == fileId),
            "practice" => await _dbContext.PracticeFiles.FirstOrDefaultAsync(f => f.Id == fileId),
            "seminar" => await _dbContext.SeminarFiles.FirstOrDefaultAsync(f => f.Id == fileId),
            "homework" => await _dbContext.HomeTaskFiles.FirstOrDefaultAsync(f => f.Id == fileId),
            _ => throw new Exception("Invalid type.")
        };

        if (file == null || string.IsNullOrEmpty(file.FilePath))
            throw new Exception("File not found or missing path.");

        var physicalPath = Path.Combine(_env.WebRootPath, file.FilePath.TrimStart('/'));

        if (!System.IO.File.Exists(physicalPath))
            throw new Exception("File does not exist on the server.");

        return new FileDownloadInfo
        {
            FilePath = physicalPath,
            ContentType = (file as dynamic).ContentType ?? "application/octet-stream", // fallback
            FileName = Path.GetFileName(physicalPath)
        };
    }

    public async Task DeleteMaterial(int fileId, string fileType)
    {
        object? file = fileType.ToLower() switch
        {
            "lecture" => await _dbContext.LectureFiles.FirstOrDefaultAsync(f => f.Id == fileId),
            "practice" => await _dbContext.PracticeFiles.FirstOrDefaultAsync(f => f.Id == fileId),
            "seminar" => await _dbContext.SeminarFiles.FirstOrDefaultAsync(f => f.Id == fileId),
            "homework" => await _dbContext.HomeTaskFiles.FirstOrDefaultAsync(f => f.Id == fileId),
            _ => throw new Exception("Invalid type.")
        };
         _dbContext.Remove(file);
         await _dbContext.SaveChangesAsync();
    }


    // public async Task Put(string id, IFormFile file)
    // {
    //     var user = await _dbContext.Users.FindAsync(id);
    //     if (user == null)
    //         throw new HttpException(StatusCodes.Status400BadRequest,
    //             "Вы ввели id неверно");
    //
    //     var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
    //     var filePath = Path.Combine("wwwroot/images", fileName);
    //
    //     using (var stream = new FileStream(filePath, FileMode.Create))
    //     {
    //         await file.CopyToAsync(stream);
    //     }
    //
    //     user.ProfileImagePath = $"/images/{fileName}";
    //     await _dbContext.SaveChangesAsync();
    // }
}