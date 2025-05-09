namespace GradeCom.Models;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; }

    public List<Grade> Grades { get; set; }
}