namespace LearnToDev.Mvc.Models;

public class JobPrerequisite
{
    public int JobId { get; set; }
    public Job Job { get; set; } = null!;

    public int RequiredJobId { get; set; }
    public Job RequiredJob { get; set; } = null!;
}