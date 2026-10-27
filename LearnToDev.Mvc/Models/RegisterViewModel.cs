using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Mvc.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Skriv inn et brukernavn.")]
    [Display(Name = "Brukernavn")]
    public string UserName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Skriv inn e-post.")]
    [EmailAddress(ErrorMessage = "Ugyldig e-postadresse.")]
    [Display(Name = "E-post")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Skriv inn passord.")]
    [DataType(DataType.Password)]
    [Display(Name = "Passord")]
    public string Password { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Gjenta passordet.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passordene er ikke like.")]
    [Display(Name = "Gjenta passord")]
    public string ConfirmPassword { get; set; } = string.Empty;
}