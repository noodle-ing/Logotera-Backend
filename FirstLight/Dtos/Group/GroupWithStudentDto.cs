using FirstLight.Dtos.StudentDto;

namespace FirstLight.Dtos.Group;

public class GroupWithStudentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<StudentUserDto> Students { get; set; }
}