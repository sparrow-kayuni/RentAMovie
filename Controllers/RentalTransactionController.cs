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
    public class RentalTransactionController : Controller
    {
        private readonly RentAmovieSystemMod2Context _context;

        public RentalTransactionController(RentAmovieSystemMod2Context context)
        {
            _context = context;
        }

        // GET: RentalTransaction
        public async Task<IActionResult> Index()
        {
            var rentAmovieSystemMod2Context = _context.RentalTransactions.Include(r => r.Customer).Include(r => r.Movie).Include(r => r.Session);
            return View(await rentAmovieSystemMod2Context.ToListAsync());
        }

        // GET: RentalTransaction/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.RentalTransactions == null)
            {
                return NotFound();
            }

            var rentalTransaction = await _context.RentalTransactions
                .Include(r => r.Customer)
                .Include(r => r.Movie)
                .Include(r => r.Session)
                .FirstOrDefaultAsync(m => m.RentalId == id);
            if (rentalTransaction == null)
            {
                return NotFound();
            }

            return View(rentalTransaction);
        }

        // GET: RentalTransaction/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "MovieId");
            ViewData["SessionId"] = new SelectList(_context.LoginSessions, "SessionId", "SessionId");
            return View();
        }

        // POST: RentalTransaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentalId,RentalDay,ReturnDate,CustomerId,SessionId,MovieId")] RentalTransaction rentalTransaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rentalTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", rentalTransaction.CustomerId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "MovieId", rentalTransaction.MovieId);
            ViewData["SessionId"] = new SelectList(_context.LoginSessions, "SessionId", "SessionId", rentalTransaction.SessionId);
            return View(rentalTransaction);
        }

        // GET: RentalTransaction/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.RentalTransactions == null)
            {
                return NotFound();
            }

            var rentalTransaction = await _context.RentalTransactions.FindAsync(id);
            if (rentalTransaction == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", rentalTransaction.CustomerId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "MovieId", rentalTransaction.MovieId);
            ViewData["SessionId"] = new SelectList(_context.LoginSessions, "SessionId", "SessionId", rentalTransaction.SessionId);
            return View(rentalTransaction);
        }

        // POST: RentalTransaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("RentalId,RentalDay,ReturnDate,CustomerId,SessionId,MovieId")] RentalTransaction rentalTransaction)
        {
            if (id != rentalTransaction.RentalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentalTransaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalTransactionExists(rentalTransaction.RentalId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", rentalTransaction.CustomerId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "MovieId", rentalTransaction.MovieId);
            ViewData["SessionId"] = new SelectList(_context.LoginSessions, "SessionId", "SessionId", rentalTransaction.SessionId);
            return View(rentalTransaction);
        }

        // GET: RentalTransaction/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.RentalTransactions == null)
            {
                return NotFound();
            }

            var rentalTransaction = await _context.RentalTransactions
                .Include(r => r.Customer)
                .Include(r => r.Movie)
                .Include(r => r.Session)
                .FirstOrDefaultAsync(m => m.RentalId == id);
            if (rentalTransaction == null)
            {
                return NotFound();
            }

            return View(rentalTransaction);
        }

        // POST: RentalTransaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.RentalTransactions == null)
            {
                return Problem("Entity set 'RentAmovieSystemMod2Context.RentalTransactions'  is null.");
            }
            var rentalTransaction = await _context.RentalTransactions.FindAsync(id);
            if (rentalTransaction != null)
            {
                _context.RentalTransactions.Remove(rentalTransaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalTransactionExists(long id)
        {
          return (_context.RentalTransactions?.Any(e => e.RentalId == id)).GetValueOrDefault();
        }
    }
}
