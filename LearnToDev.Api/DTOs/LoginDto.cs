using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Api.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Skriv inn e-post eller brukernavn.")]
    public string EmailOrUserName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Skriv inn passord.")]
    public string Password { get; set; } = string.Empty;
    
    public bool RememberMe { get; set; }
}