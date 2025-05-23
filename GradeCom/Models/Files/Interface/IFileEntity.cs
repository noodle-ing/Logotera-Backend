namespace GradeCom.Models.Files.Interface;

public interface IFileEntity
{
    int Id { get; set; }
    string FileName { get; set; }
    string FilePath { get; set; }
    public string ContentType { get; set; } 
}