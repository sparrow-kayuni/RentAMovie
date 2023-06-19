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
    public class CustomerController : Controller
    {
        private readonly RentAmovieSystemMod2Context _context;

        public CustomerController(RentAmovieSystemMod2Context context)
        {
            _context = context;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            var rentAmovieSystemMod2Context = _context.Customers;
            return View(await rentAmovieSystemMod2Context.ToListAsync());
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Address)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            // ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId");
            ViewData["HouseAddress"] = "";
            ViewData["ZipCode"] = "";
            ViewData["City"] = "";
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FName,MName,LName,Email,PhoneNo")] Customer customer,
        [Bind("AddressId,HouseAddress,ZipCode,City,CustomerId")] Address address)
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                try{
                    customer.Address = address;
                    address.Customer = customer;
                    
                    _context.Add(customer);
                    _context.Add(address);

                    await _context.SaveChangesAsync();

                } catch(DbUpdateConcurrencyException)
                {
                    Console.WriteLine("An error saving happened");
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CustomerId,FName,MName,LName,Email,PhoneNo,AddressId")] Customer customer)
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Address)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            TempData.Keep("Session_Key");

            if(!SessionExists(TempData["Session_Key"].ToString()))
            {
                return RedirectToAction("Index", "Login");
            }
            
            if (_context.Customers == null)
            {
                return Problem("Entity set 'RentAmovieSystemMod2Context.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(long id)
        {
          return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }

        public bool SessionExists(string key)
        {
            return _context.LoginSessions
                .Any(m => String.Equals(m.SessionKey, key) && m.TimeEnded == null);
        }
    }
}
