using System.ComponentModel.DataAnnotations;

namespace Logotera.Models;

public class WeeklyReport
{
    public int Id { get; set; }

    [Required]
    public DateTime WeekStart { get; set; }

    [Required]
    public string Text { get; set; } = "";
}