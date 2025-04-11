using GradeCom.Enum;

namespace GradeCom.Models;

public class Grade
{
    public int Id { get; set; }
    public GradeType Type { get; set; }    
    public short Value { get; set; }
    public DateTime Date { get; set; }
}