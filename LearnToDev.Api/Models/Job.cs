using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Mvc.Models;

public class Job
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Rank { get; set; }

    [Range(0, int.MaxValue)]
    public int Wage { get; set; }

    public int HoursPerShift { get; set; } = 4;

    [Range(1, int.MaxValue)]
    public int TasksPerDay { get; set; }

    public ICollection<JobRequirement> JobRequirements { get; set; } = new List<JobRequirement>();
    public ICollection<JobTask> JobTasks { get; set; } = new List<JobTask>();

    //Jobs the player must have worked in before taking this job
    public ICollection<JobPrerequisite> Prerequisites { get; set; } = new List<JobPrerequisite>();
    //Jobs that require the player to have worked in this job first
    public ICollection<JobPrerequisite> RequiredBy { get; set; } = new List<JobPrerequisite>();

    public ICollection<JobDay> JobDays { get; set; } = new List<JobDay>();
    public ICollection<Player> Players { get; set; } = new List<Player>();
}