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
    public class MovieController : Controller
    {
        private readonly RentAmovieSystemMod2Context _context;

        public MovieController(RentAmovieSystemMod2Context context)
        {
            _context = context;
        }

        // GET: Movie
        public async Task<IActionResult> Index()
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }
            
            return _context.Movies != null ? 
                        View(await _context.Movies.ToListAsync()) :
                        Problem("Entity set 'RentAmovieSystemMod2Context.Movies'  is null.");
        }

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movie/Create
        public IActionResult Create()
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,Title,YearOfRelease,UnitPrice")] Movie movie)
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("MovieId,Title,YearOfRelease,UnitPrice")] Movie movie)
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
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
            return View(movie);
        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            if (_context.Movies == null)
            {
                return Problem("Entity set 'RentAmovieSystemMod2Context.Movies'  is null.");
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(long id)
        {
          return (_context.Movies?.Any(e => e.MovieId == id)).GetValueOrDefault();
        }

        public bool SessionExists(string key)
        {
            return _context.LoginSessions
                .Any(m => String.Equals(m.SessionKey, key) && m.TimeEnded == null);
        }
    }
}
