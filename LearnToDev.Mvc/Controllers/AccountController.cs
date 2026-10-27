using LearnToDev.Mvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnToDev.Mvc.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    // håndterer login av allerede registrerte brukere, forhindrer brute forcing ved hjelp av PasswordSignInAsync
    // SignInAsync lager en innloggingscookie
    
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var user = await _userManager.FindByEmailAsync(model.EmailOrUserName) 
                   ?? await _userManager.FindByNameAsync(model.EmailOrUserName);
        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(
                user, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                _logger.LogInformation("Bruker {UserId} logget inn.", user.Id);
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("Bruker {UserId} er låst ute etter for mange feilforsøk.", user.Id);
                ModelState.AddModelError(string.Empty, "For mange feilforsøk. Prøv igjen om noen minutter.");
                return View(model);
            }
        }
        ModelState.AddModelError(string.Empty, "Ugyldig innlogging.");
        return View(model);
    }
    
    // Håndterer registrering av brukeren, og gjør noen sjekker.
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new IdentityUser
        {
            UserName = model.UserName,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("Ny bruker {UserId} registrert.", user.Id);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }
    
    // logger ut brukeren (SignOutAsync sletter innloggingscookien) 
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        var userId = _userManager.GetUserId(User);
        await _signInManager.SignOutAsync();
        _logger.LogInformation("Bruker {UserId} logget ut.", userId);
        return RedirectToAction("Index", "Home");
    }
    // vises når en innlogget bruker mangler rolle til en side (Identity sender hit automatisk)
    
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}