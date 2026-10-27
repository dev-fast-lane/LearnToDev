using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LearnToDev.Mvc.Models;

public class Player
{
    public int Id { get; set; }

    //Link to the Identity login user. Each player has exactly one user
    [Required]
    public string UserId { get; set; } = string.Empty;
    public IdentityUser User { get; set; } = null!;

    [Range(0, int.MaxValue)]
    public int Money { get; set; }

    [Range(1, int.MaxValue)]
    public int Day { get; set; } = 1;
    [Range(0, 24)]
    public int HoursLeftInDay { get; set; }

    //The housing the player lives in (HousingId is stored in the database, Housing gives the object)
    public int HousingId { get; set; }
    public Housing Housing { get; set; } = null!;

    //Current job. null means the player is unemployed
    public int? CurrentJobId { get; set; }
    public Job? CurrentJob { get; set; }

    //The player's progress in schools and completed job days (one player has many of each)
    public ICollection<PlayerSchoolProgress> SchoolProgresses { get; set; } = new List<PlayerSchoolProgress>();
    public ICollection<JobDay> JobDays { get; set; } = new List<JobDay>();
}