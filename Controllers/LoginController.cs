using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentAMovie_v3.Models;

namespace RentAMovie_v3.Controllers
{
    public class LoginController : Controller
    {
        private readonly RentAmovieSystemMod2Context _context;

        public LoginController(RentAmovieSystemMod2Context context)
        {
            _context = context;
        }

        // GET: Login
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(string username, string password)
    {
        // if (!_loginService.verifyLoginCredentials(username, password))
        // {
        //     return View();
        // }

        // LoginSession session = _loginService.createLoginSession();
        return RedirectToAction(nameof(HomeController.AdminDash));
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

        // GET: Login/Create
        public IActionResult Create()
        {
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId");
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SessionId,TimeStarted,TimeEnded,StaffId")] LoginSession loginSession)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loginSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId", loginSession.StaffId);
            return View(loginSession);
        }

        // GET: Login/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.LoginSessions == null)
            {
                return NotFound();
            }

            var loginSession = await _context.LoginSessions.FindAsync(id);
            if (loginSession == null)
            {
                return NotFound();
            }
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId", loginSession.StaffId);
            return View(loginSession);
        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SessionId,TimeStarted,TimeEnded,StaffId")] LoginSession loginSession)
        {
            if (id != loginSession.SessionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loginSession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoginSessionExists(loginSession.SessionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId", loginSession.StaffId);
            return View(loginSession);
        }

        // GET: Login/Delete/5
        public async Task<IActionResult> Delete(long? id)
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

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.LoginSessions == null)
            {
                return Problem("Entity set 'RentAmovieSystemMod2Context.LoginSessions'  is null.");
            }
            var loginSession = await _context.LoginSessions.FindAsync(id);
            if (loginSession != null)
            {
                _context.LoginSessions.Remove(loginSession);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoginSessionExists(long id)
        {
          return (_context.LoginSessions?.Any(e => e.SessionId == id)).GetValueOrDefault();
        }
    }
}