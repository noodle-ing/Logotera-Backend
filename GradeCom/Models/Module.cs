using GradeCom.Models.Files;

namespace GradeCom.Models;

public class Module
{
    public int Id { get; set; }
    public string Title { get; set; }               
    public string? Description { get; set; }     

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }

    public ICollection<LectureFile>? LectureFiles { get; set; }
    public ICollection<SeminarFile>? SeminarFiles { get; set; }
    public ICollection<PracticeFile>? PracticeFiles { get; set; }
    public ICollection<HomeTaskFile>? HomeTaskFiles { get; set; }
}