using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LearnToDev.Mvc.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace LearnToDev.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        
        if (feature != null)
        {
            _logger.LogError(feature.Error, "Uventet feil på {Path}.", feature.Path);
        }
        
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    public IActionResult Status(int code)
    {
        var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
        
        if (code == 404)
        {
            _logger.LogWarning("Fant ikke siden {Path}.", feature?.OriginalPath);
            return View("NotFound");
        }
        return View("Error");
    }
}
