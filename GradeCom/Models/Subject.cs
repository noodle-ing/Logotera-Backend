using GradeCom.Services.AuthenticationServices;

namespace GradeCom.Models;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Group>? Groups { get; set; }
    public int LecturerTeacherId { get; set; }
    public int PracticeTeacherId { get; set; }
    
    public Teacher LecturerTeacher { get; set; }
    public Teacher PracticeTeacher { get; set; }
    public List<Grade>? Grades { get; set; }
}