namespace GradeCom.Dtos.ModuleDto;

public class CreateModuleDto
{
    public string Title { get; set; }               
    public string? Description { get; set; }     

    public int SubjectId { get; set; }
}