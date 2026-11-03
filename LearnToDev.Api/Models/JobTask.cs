using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Mvc.Models;

public class JobTask
{
    public int Id { get; set; }

    public int JobId { get; set; }
    public Job Job { get; set; } = null!;

    [Required, StringLength(3000)]
    public string Text { get; set; } = string.Empty;

    public ICollection<JobTaskAnswer> JobTaskAnswers { get; set; } = new List<JobTaskAnswer>();
}