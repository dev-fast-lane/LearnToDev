using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Mvc.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Skriv inn e-post eller brukernavn.")]
    [Display(Name = "E-post eller brukernavn")]
    public string EmailOrUserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Skriv inn passord.")]
    [DataType(DataType.Password)]
    [Display(Name = "Passord")]
    public string Password { get; set; } = string.Empty;
    
    [Display(Name = "Husk meg")]
    public bool RememberMe { get; set; }
}