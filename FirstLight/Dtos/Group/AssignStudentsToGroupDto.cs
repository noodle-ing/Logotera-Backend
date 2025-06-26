namespace FirstLight.Dtos.Group;

public class AssignStudentsToGroupDto
{
    public int GroupId { get; set; }
    public List<int> StudentIds { get; set; }
}