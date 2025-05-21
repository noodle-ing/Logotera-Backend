namespace GradeCom.Dtos.File;

public class FileDownloadInfo
{
    public string FilePath { get; set; } = default!;
    public string ContentType { get; set; } = "application/octet-stream";
    public string FileName { get; set; } = default!;
}