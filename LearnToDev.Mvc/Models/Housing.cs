using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Mvc.Models;

public class Housing
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Order { get; set; }

    //Tenkte fra 0 til 24 timer pga antall timer per dag
    [Range(0, 24)]
    public int HoursPerDay { get; set; }

    [Range(0, int.MaxValue)]
    public int Rent { get; set; }
}