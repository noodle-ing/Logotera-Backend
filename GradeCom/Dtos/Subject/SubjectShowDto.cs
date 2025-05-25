using GradeCom.Dtos.Group;
using GradeCom.Dtos.ModuleDto;
using GradeCom.Dtos.Teacher;
using GradeCom.Models;

namespace GradeCom.Dtos.Subject;

public class SubjectShowDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TeacherDto? LecturerTeacher { get; set; }
    public TeacherDto? PracticeTeacher { get; set; }
    public List<GroupViewDto> Groups { get; set; } = new();
    public string? SyllabusFilePath { get; set; }
    public List<ModuleViewDto> Modules { get; set; } = new();
}