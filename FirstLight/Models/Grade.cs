using FirstLight.Enum;

namespace FirstLight.Models;

public class Grade
{
    public int Id { get; set; }
    public GradeType Type { get; set; }    
    public short Value { get; set; }
    public DateTime Date { get; set; }
    
    public int StudentId { get; set; }
    public Student Studen{ get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }
}