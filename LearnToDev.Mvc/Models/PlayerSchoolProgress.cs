using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Mvc.Models;

public class PlayerSchoolProgress
{
    public int PlayerId { get; set; }
    public Player Player { get; set; } = null!;

    public int SchoolId { get; set; }
    public School School { get; set; } = null!;

    [Range(0, 3)]
    public int LevelPassed { get; set; }
}