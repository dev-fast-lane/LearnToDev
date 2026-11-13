using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Api.DTOs;

//What admin sends when creating or editing a housing. No Id: it comes from the URL or the database
public class SaveHousingDto
{
    [Required(ErrorMessage = "Skriv inn et navn")]
    [StringLength(40, ErrorMessage = "Navnet kan være maks 40 tegn.")]
    public string Name { get; set; } = string.Empty;
    [Range(1, int.MaxValue, ErrorMessage = "Rekkefølgen må være 1 eller mer")]
    public int Order { get; set; }
    [Range(0, 23, ErrorMessage = "Timer per dag må være mellom 0 og 23")]
    public int HoursPerDay { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "Husleien kan ikke være negativ.")]
    public int Rent { get; set; }
}