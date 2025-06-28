using System.ComponentModel.DataAnnotations;

namespace Logotera.Models;

public class TaskItem
{
    public int Id { get; set; }

    [Required]
    public string Text { get; set; } = "";

    public bool IsCompleted { get; set; }

    [Required]
    public DateTime Date { get; set; }
}