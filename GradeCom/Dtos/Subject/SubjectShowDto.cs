using GradeCom.Dtos.Group;
using GradeCom.Dtos.Teacher;

namespace GradeCom.Dtos.Subject;

public class SubjectShowDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TeacherDto? LecturerTeacher { get; set; }
    public TeacherDto? PracticeTeacher { get; set; }
    public List<GroupViewDto> Groups { get; set; } = new();

}