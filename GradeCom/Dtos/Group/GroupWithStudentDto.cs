using GradeCom.Dtos.StudentDto;

namespace GradeCom.Dtos.Group;

public class GroupWithStudentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<StudentUserDto> Students { get; set; }
}