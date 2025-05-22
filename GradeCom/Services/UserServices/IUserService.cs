using GradeCom.Dtos.File;
using GradeCom.Dtos.Group;
using GradeCom.Dtos.ModuleDto;
using GradeCom.Dtos.StudentDto;
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
    Task DeleteTeacherFromSubject(TeacherDeleteFromSubjectDto toSubjectDto);
    Task DeleteGroupFromSubject(GroupAddToSubjectDto toSubjectDto);
    Task DeleteSubject(int subjectId);
    Task DeleteGroup(int groupId);
    Task<List<SubjectShowDto>> GetAllSubjects();
    Task<List<User>> GetAllTeachers();
    Task<List<StudentUserDto>> GetAllStudent();
    Task<List<GroupWithStudentDto>> GetAllGroups();
    Task<List<Subject>> GetAllTeacherSubject(string email);
    Task<SubjectShowDto> GetSubjectForTeacher(int subjectId);
    Task AddSyllabus(int subjectId, IFormFile file);
    Task<FileDownloadInfo> GetSyllabus(int subjectId);
    Task CreateModule(CreateModuleDto module);
    Task DeleteModule(int moduleId);
    Task EditModule(ModuleViewDto moduleDto);
    Task UploadMaterialFile(int moduleId, string type, IFormFile file );



}