using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RentAMovie_v3.Models;

namespace RentAMovie_v3.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ILoginService _loginService;

    public HomeController(ILogger<HomeController> logger, ILoginService loginService)
    {
        _logger = logger;
        _loginService = loginService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        // if (!_loginService.verifyLoginCredentials(username, password))
        // {
        //     return View();
        // }

        // LoginSession session = _loginService.createLoginSession();
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
