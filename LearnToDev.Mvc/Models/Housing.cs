using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Mvc.Models;

public class Housing
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Order { get; set; }

    //Total number of hours the player has per day in this housing (0-24)
    [Range(0, 23)]
    public int HoursPerDay { get; set; }

    [Range(0, int.MaxValue)]
    public int Rent { get; set; }

    //Players living here (one housing has many players)
    public ICollection<Player> Players { get; set; } = new List<Player>();
}