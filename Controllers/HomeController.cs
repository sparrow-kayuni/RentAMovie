using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RentAMovie_v3.Models;

namespace RentAMovie_v3.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RentAmovieSystemMod2Context _context;

    public HomeController(ILogger<HomeController> logger, RentAmovieSystemMod2Context context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {      
        // Check if user has loggged in, if not, redirect to Login page
        TempData.Keep("Session_Key");
        Console.WriteLine("Session: {0}", TempData["Session_Key"].ToString());

        if(!SessionExists(TempData["Session_Key"].ToString()))
        {
            return RedirectToAction("Index", "Login");
        }

        return View();        
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public bool SessionExists(string key)
    {
        return _context.LoginSessions
            .Any(m => String.Equals(m.SessionKey, key) && m.TimeEnded == null);
    }
}
