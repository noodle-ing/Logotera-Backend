using GradeCom.Dtos.File;

namespace GradeCom.Dtos.ModuleDto;

public class ModuleViewDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<FileDownloadInfo> LectureMaterial = new();
    public List<FileDownloadInfo> PracticeMaterial = new();
    public List<FileDownloadInfo> SeminarMaterial = new();
    public List<FileDownloadInfo> HomeMaterial = new();
}