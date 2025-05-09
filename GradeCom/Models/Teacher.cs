namespace GradeCom.Models;

public class Teacher
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public List<Group>? Groups { get; set; }
    public int? SubjectId { get; set; }
    public Subject? Subject { get; set; }
}