using GradeCom.Dtos.Group;
using GradeCom.Dtos.Subject;
using GradeCom.Dtos.UserDtos;
using GradeCom.Models;

namespace GradeCom.Services.UserServices;

public interface IUserService
{
    Task<UserDto> Get(string id);
    Task<UserDto> Post(UserDto userDto);
    Task<UserDto> Put(UserDto userDto);
    Task<UserDto> Patch(string id, string description);
    Task Delete(string id);
    Task<List<User>> GetAllUsersAsync(string userId);
    Task CreateGroup(GroupCreateDto group);
    Task AddStudentToGroup(AssignStudentsToGroupDto dto);
    Task DeleteStudentFromGroup(AssignStudentsToGroupDto dto);
    Task CreateSubject(SubjectCreateDto dto);
    Task AddTeacherToSubject(TeacherAddDto dto);
    Task AddGroupToSubject(GroupAddToSubjectDto toSubjectDto);
    Task DeleteTeacherFromSubject(TeacherAddDto toSubjectDto);
    Task DeleteGroupFromSubject(GroupAddToSubjectDto toSubjectDto);

}