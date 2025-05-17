namespace GradeCom.Dtos.Subject;

public class SubjectCreateDto
{
    public string SubjectName { get; set; }
    public IFormFile? Syllabus { get; set; } 
}