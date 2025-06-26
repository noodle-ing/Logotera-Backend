using FirstLight.Dtos.File;

namespace FirstLight.Dtos.ModuleDto;

public class ModuleViewDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public List<FileDownloadInfo> LectureMaterial { get; set; } = new();
    public List<FileDownloadInfo> PracticeMaterial { get; set; } = new();
    public List<FileDownloadInfo> SeminarMaterial { get; set; } = new();
    public List<FileDownloadInfo> HomeMaterial { get; set; } = new();
}