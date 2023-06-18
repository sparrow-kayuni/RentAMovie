using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RentAMovie_v3.Models;

namespace RentAMovie_v3.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // check if user has logged in, if not, redirect to login


        return RedirectToAction(nameof(AdminDash));
    }

    [HttpGet]
    public IActionResult AdminDash()
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
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
