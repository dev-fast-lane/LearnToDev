namespace LearnToDev.Mvc.Models;

public class JobRequirement
{
    public int JobId { get; set; }
    public Job Job { get; set; } = null!;

    public int SchoolLevelId { get; set; }
    public SchoolLevel SchoolLevel { get; set; } = null!;
}