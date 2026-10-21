using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Mvc.Models;

public class JobTaskAnswer
{
    public int Id { get; set; }

    public int JobTaskId { get; set; }
    public JobTask JobTask { get; set; } = null!;

    [Required]
    public string Text { get; set; } = string.Empty;
}   