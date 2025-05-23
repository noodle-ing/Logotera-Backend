using GradeCom.Models.Files.Interface;

namespace GradeCom.Models.Files;

public class HomeTaskFile : IFileEntity
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string ContentType { get; set; }

    public int ModuleId { get; set; }
    public Module Module { get; set; }
}