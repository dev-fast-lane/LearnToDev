using System.Security.Claims;
using LearnToDev.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnToDev.Api.Controllers;

// Endpoints only admins can use. The role check is on the class, so every new action here is protected
[ApiController]
[Route("api/admin")]
[Authorize(Roles = Roles.Admin)]
public class AdminController : ControllerBase
{
    private readonly ILogger<AdminController> _logger;

    public AdminController(ILogger<AdminController> logger)
    {
        _logger = logger;
    }

    // Shows that the role check works: 401 when not logged in, 403 for players, 200 for admins
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        _logger.LogInformation("Admin {UserId} called ping", userId);
        return Ok(new { message = "Hello, admin" });
    }
}