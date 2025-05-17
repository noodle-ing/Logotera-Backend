namespace GradeCom.Models.Files;

public class PracticeFile
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }

    public int ModuleId { get; set; }
    public Module Module { get; set; }
}