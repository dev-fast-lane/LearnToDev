using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Mvc.Models;

public class JobDay
{
    public int Id { get; set; }

    public int PlayerId { get; set; }
    public Player Player { get; set; } = null!;

    public int JobId { get; set; }
    public Job Job { get; set; } = null!;

    [Range(0, 365)]
    public int Day { get; set; }

    [Range(0, 10)]
    public int TasksTotal { get; set; }

    public int TasksCorrect { get; set; }

    public bool Completed { get; set; }
}