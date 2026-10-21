using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LearnToDev.Mvc.Models;

public class Player
{
    public int Id { get; set; }

    //Koblingen til Identity-brukeren
    [Required]
    public string UserId { get; set; } = string.Empty;
    public IdentityUser User { get; set; } = null!;

    [Range(0, int.MaxValue)]
    public int Money { get; set; }

    public int Day { get; set; } = 1;

    //Bolig: fremmednøkkel + navigasjon
    public int HousingId { get; set; }
    public Housing Housing { get; set; } = null!;

    //null = arbeidsledig
    public int? CurrentJobId { get; set; }
    public Job? CurrentJob { get; set; }

    //Lister til "Mange"-siden av relasjonene

    public ICollection<PlayerSchoolProgress> SchoolProgresses { get; set; } = new List<PlayerSchoolProgress>();
    public ICollection<JobDay> JobDays { get; set; } = new List<JobDay>();
}