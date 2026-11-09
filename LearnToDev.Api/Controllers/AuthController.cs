using LearnToDev.Api.DTOs;
using LearnToDev.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LearnToDev.Api.Controllers;

//Innlogging, registrering og utlogging for React. Svarer med JSON og statuskoder, ikke views.
//[ApiController] sjekker valideringen i DTO-ene automatisk og svarer 400 hvis den feiler
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<AuthController> _logger;

    public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
        ILogger<AuthController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    //Logger inn med e-post eller brukernavn, og setter innloggingscookien hvis passordet stemmer
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        //Søker først på e-post, og deretter på brukernavn hvis det ikke ga treff
        var user = await _userManager.FindByEmailAsync(dto.EmailOrUserName)
                   ?? await _userManager.FindByNameAsync(dto.EmailOrUserName);
        if (user != null)
        {
            //lockoutOnFailure låser kontoen etter for mange feil passord, så ingen kan gjette seg frem
            var result = await _signInManager.PasswordSignInAsync(
                user, dto.Password, dto.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("Bruker {UserId} logget inn.", user.Id);
                return Ok(await ToUserDtoAsync(user));
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("Bruker {UserId} er låst ute etter for mange feilforsøk.", user.Id);
                return Unauthorized(new { message = "For mange feilforsøk. Prøv igjen om noen minutter." });
            }
        }
        //Samme svar om brukeren ikke finnes eller passordet er feil, så ingen kan finne ut hvilke brukere som finnes
        return Unauthorized(new { message = "Ugyldig innlogging." });
    }

    //Oppretter en ny bruker og logger den inn med en gang
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new IdentityUser
        {
            UserName = dto.UserName,
            Email = dto.Email
        };

        //Hasher passordet og sjekker det DTO-en ikke kan vite, som passordstyrke og om navnet er ledig
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("Ny bruker {UserId} registrert.", user.Id);

            //Alle nye brukere er spillere. Må skje før innlogging, så rollen kommer med i cookien
            await _userManager.AddToRoleAsync(user, Roles.Player);

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok(await ToUserDtoAsync(user));
        }

        //Feilene fra Identity sendes i samme format som valideringsfeilene, så React kan vise dem likt
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return ValidationProblem(ModelState);
    }

    //Sletter innloggingscookien
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        //Må hentes før utlogging, for etterpå er id-en borte
        var userId = _userManager.GetUserId(User);
        await _signInManager.SignOutAsync();
        _logger.LogInformation("Bruker {UserId} logget ut.", userId);
        return NoContent();
    }

    //Lar React sjekke om brukeren er innlogget når siden lastes
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var user = await _userManager.GetUserAsync(User);

        //Cookien kan være gyldig selv om brukeren er slettet fra databasen
        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(await ToUserDtoAsync(user));
    }

    //Lager svaret til React. Bruker en DTO så passord-hashen aldri blir sendt
    private async Task<UserDto> ToUserDtoAsync(IdentityUser user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Roles = await _userManager.GetRolesAsync(user)
        };
    }
}