using System.Text.RegularExpressions;

namespace GradeCom.Models;

public class Student
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; }
    public List<Grade> Grades { get; set; }
}