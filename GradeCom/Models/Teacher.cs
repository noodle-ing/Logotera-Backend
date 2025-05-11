namespace GradeCom.Models;

public class Teacher
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public List<SubjectTeacher> SubjectTeachers { get; set; } = new();
    
}