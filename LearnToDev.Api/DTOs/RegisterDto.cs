using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Api.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Skriv inn et brukernavn.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Skriv inn e-post.")]
    [EmailAddress(ErrorMessage = "Ugyldig e-postadresse.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Skriv inn passord.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gjenta passordet.")]
    [Compare(nameof(Password), ErrorMessage = "Passordene er ikke like.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}