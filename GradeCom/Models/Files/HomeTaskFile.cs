namespace GradeCom.Models.Files;

public class HomeTaskFile
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }

    public int ModuleId { get; set; }
    public Module Module { get; set; }
}