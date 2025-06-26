using FirstLight.Dtos.Group;
using FirstLight.Dtos.ModuleDto;
using FirstLight.Dtos.Teacher;
using FirstLight.Models;

namespace FirstLight.Dtos.Subject;

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