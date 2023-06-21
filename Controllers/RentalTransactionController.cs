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
        private Customer currentCustomer;

        private RentalTransaction currentTransaction;
        private List<RentalTransaction> rentalTransactions;
        private ItemList<Movie> moviesList = new ItemList<Movie>(); 
        private LoginSession session;

        public RentalTransactionController(RentAmovieSystemMod2Context context)
        {
            _context = context;
        }

        // GET: RentalTransaction
        public async Task<IActionResult> Index()
        {
            // Check if user has loggged in, if not, redirect to Login page
            TempData.Keep("Session_Key");
            Console.WriteLine("Session: {0}", TempData["Session_Key"].ToString());

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            var rentAmovieSystemMod2Context = _context.RentalTransactions.Include(r => r.Customer).Include(r => r.Movie).Include(r => r.Session);
            return View(await rentAmovieSystemMod2Context.ToListAsync());
        }

        // Create a new Rental transaction
        public async Task<IActionResult> NewTransaction(
            string searchString, 
            string sortOrder,
            string currentFilter,
            int? pageNumber,
            long id = 0)
        {
            // Check if user has loggged in, if not, redirect to Login page
            TempData.Keep("Session_Key");
            Console.WriteLine("Session: {0}", TempData["Session_Key"].ToString());

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            session = await _context.LoginSessions
                .FirstOrDefaultAsync(m => String.Equals(m.SessionKey, TempData["Session_Key"]
                .ToString()));

            // check if customers list is empty
            if (_context.Customers == null)
             {
                     return Problem("Entity set 'RentAmovieContext.Movie'  is null.");
             }

            var customers = from m in _context.Customers.Include(c => c.Address)
            select m;
    
            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(s => s.LName!.Contains(searchString) || s.FName!.Contains(searchString));
            }

            // if a customer id is given, query the customers table for the customer
            if (id != 0)
            {
                currentCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
            }

            rentalTransactions = new List<RentalTransaction>();

            int pageSize =  20;

            return View(await PaginatedList<Customer>
            .CreateAsync(customers.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> SelectMovie(
            long? id, 
            string searchString, 
            long? movieId,
            long? rentalId,
            int? pageNumber)
        {
            TempData.Keep("Session_Key");
            Console.WriteLine("Session: {0}", TempData["Session_Key"].ToString());
            
            // Check if user has loggged in, if not, redirect to Login page
            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            // Check iff there are movies
            if (_context.Movies == null)
            {
                return Problem("Entity set 'RentAmovieContext.Movie'  is null.");
            }
            
            // Get all movies
            var movies = from m in _context.Movies select m;

            // if a customer id is given, query the customers table for the customer
            if (id != 0)
            {
                currentCustomer = _context.Customers.FirstOrDefault(c => c.CustomerId == id);
            }

            // when moie is searched for
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title!.Contains(searchString));
            }
            
            var session = await _context.LoginSessions.FirstOrDefaultAsync(m => m.SessionKey == TempData["Session_Key"].ToString());
            
            // if a movie is added 
            if(movieId != null)
            {
                // if a rental id is gien, delete it
                if(rentalId != null)
                {
                    var rentalTxn = await _context.RentalTransactions
                    .FirstOrDefaultAsync(m => m.RentalId == rentalId && 
                    m.MovieId == movieId && m.CustomerId == currentCustomer.CustomerId);
                    
                    _context.RentalTransactions.Remove(rentalTxn);
                    
                    await _context.SaveChangesAsync();
                } 
                else 
                {
                    var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == movieId);
                    
                    // Create rental transaction then add in to renta transactions list
                    var rental = new RentalTransaction(){
                        Movie = movie,
                        Customer = currentCustomer,
                        Session = session,
                        RentalDay = DateTime.Today,
                        RentalId = _context.RentalTransactions.Count() + 1
                    };

                    if (rental != null)
                    {
                        var rentalExists = _context.RentalTransactions
                        .Any(m => m.CustomerId == currentCustomer.CustomerId && m.MovieId == movie.MovieId);

                        if (!rentalExists)
                        {
                            _context.Add(rental);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                
                // Get current movie and session
                moviesList.Transactions = _context.RentalTransactions
                    .Where(m => String.Equals(m.Session.SessionKey, session.SessionKey) 
                    && m.CustomerId == currentCustomer.CustomerId).ToList();       
                
            }
            
            moviesList.SetItems(movies.ToList());

            // number of results displayed
            int pageSize =  20;

            return View(moviesList);
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

        public bool SessionExists(string key)
        {
            return _context.LoginSessions
                .Any(m => String.Equals(m.SessionKey, key) && m.TimeEnded == null);
        }
    }
}
