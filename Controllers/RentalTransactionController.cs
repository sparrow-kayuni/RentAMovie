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

            var rentalTransactions = _context.RentalTransactions.Include(r => r.Customer)
            .Include(r => r.Movie).Include(r => r.Session);
            return View(await rentalTransactions.ToListAsync());
        }

        public async Task<IActionResult> ReturnBook(long id)
        {
            if(id != 0)
            {
                var rental = _context.RentalTransactions.FirstOrDefault(m => m.RentalId == id);
                rental.ReturnDate = DateTime.UtcNow;

                _context.RentalTransactions.Update(rental);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");

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
            string key = TempData["Session_Key"].ToString();
            ViewData["Error_Message"] = "";
            
            TempData.Keep("Session_Key");

            Console.WriteLine("Session: {0}", key);
            
            // Check if user has loggged in, if not, redirect to Login page
            if(!SessionExists(key))
            {
                return RedirectToAction("Index", "Login");
            }

            // get current session
            var session = _context.LoginSessions.Include(r => r.Staff)
            .FirstOrDefault(m => String.Equals(m.SessionKey, key));
            
            System.Console.WriteLine("Session User: {0}", session.Staff.StaffUserName);

            // Check if there are movies available
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

            // when movie is searched for
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title!.Contains(searchString));
            }

            // if a movie is selected 
            if(movieId != null)
            {
                // if a rental id is given, delete the rental from the database
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
                    
                    var maxIdNo = 1L;

                    if(_context.RentalTransactions.Count() > 0){
                        maxIdNo = _context.RentalTransactions.Max(m => m.RentalId) + 1;
                    }

                    // Create rental transaction then add in to renta transactions list
                    var rental = new RentalTransaction(){
                        Movie = movie,
                        Customer = currentCustomer,
                        Session = session,
                        RentalDay = DateTime.UtcNow,
                        RentalId = maxIdNo,
                    };

                    if (rental != null)
                    {
                        var rentalExists = _context.RentalTransactions
                        .Any(m => m.MovieId == movie.MovieId && m.ReturnDate == null);

                        if (!rentalExists)
                        {
                            _context.Add(rental);
                            await _context.SaveChangesAsync();
                            
                            ViewData["Error_Message"] = "";
                        }
                        else
                        {
                            ViewData["Error_Message"] = "Movie is been rented!";
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
                .Include(r => r.Session.Staff)
                .FirstOrDefaultAsync(m => m.RentalId == id);
            if (rentalTransaction == null)
            {
                return NotFound();
            }

            return View(rentalTransaction);
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
