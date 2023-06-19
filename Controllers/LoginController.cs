using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using RentAMovie_v3.Models;

namespace RentAMovie_v3.Controllers
{
    public class LoginController : Controller
    {
        private readonly RentAmovieSystemMod2Context _context;

        public LoginController(RentAmovieSystemMod2Context context)
        {
            _context = context;
            // 
        }

        // GET: Login
        [HttpGet]
        public IActionResult Index()
        {
            TempData["Session_Key"] = "";
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            var staffUser = await _context.Staff.FirstOrDefaultAsync(c => c.StaffUserName == username);

            if (staffUser == null)
            {
                return View();
            }

            if (!String.Equals(staffUser.StaffPassword, password))
            {
                return View();
            }

            var id = _context.LoginSessions.Count();

            var loginSession = new LoginSession(){
                TimeStarted = DateTime.UtcNow,
                SessionKey = GenerateRandomString(5),
                SessionId = id + 1,
                Staff = staffUser
            };

            _context.Add(loginSession);
            await _context.SaveChangesAsync();

            TempData["Session_Key"] = loginSession.SessionKey;
            
            return Redirect(String.Format("Home"));
        }

        // Generates a random striing for a given input
        private string GenerateRandomString(int length)
        {
            string chars = "ABCDEFGHIJKKLIMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var random = new Random();
            string randomString;

            while(true)
            {
                var randomChars = new char[length];
                
                for (int i = 0; i < length; i++)
                {
                    randomChars[i] = chars[random.Next(0, 62)];
                }
                
                randomString = new String(randomChars);

                if(!SessionExists(randomString)){
                    return randomString;
                }
            }            
        }
        
        // GET: Login
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            TempData.Keep("Session_Key");
            // If session key doesnt exist, redirect to login page

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            // set the time session has ended 
            var loginSession = await _context.LoginSessions
            .FirstOrDefaultAsync(m => String.Equals(m.SessionKey, TempData["Session_Key"].ToString()));

            loginSession.TimeEnded = DateTime.UtcNow;
            _context.Update(loginSession);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Login");
        }

        // GET: Login/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.LoginSessions == null)
            {
                return NotFound();
            }

            var loginSession = await _context.LoginSessions
                .Include(l => l.Staff)
                .FirstOrDefaultAsync(m => m.SessionId == id);
            if (loginSession == null)
            {
                return NotFound();
            }

            return View(loginSession);
        }


        public bool SessionExists(string key)
        {
            return _context.LoginSessions
                .Any(m => String.Equals(m.SessionKey, key) && m.TimeEnded == null);
        }
    }
}
