using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootBallWebLaba1.Models;

namespace FootBallWebLaba1.Controllers
{
    public class StadiaController : Controller
    {
        private readonly FootBallBdContext _context;

        public StadiaController(FootBallBdContext context)
        {
            _context = context;
        }

        // GET: Stadia
        public async Task<IActionResult> Index()
        {
            var footBallBdContext = _context.Stadia.Include(s => s.Club);
            return View(await footBallBdContext.ToListAsync());
        }

        // GET: Stadia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stadia == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadia
                .Include(s => s.Club)
                .FirstOrDefaultAsync(m => m.StadiumId == id);
            if (stadium == null)
            {
                return NotFound();
            }

            return View(stadium);
        }

        // GET: Stadia/Create
        public IActionResult Create()
        {
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName");
            return View();
        }

        // POST: Stadia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StadiumId,StadiumLocation,StadiumCapacity,StadiumEstablismentDate,ClubId")] Stadium stadium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stadium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", stadium.ClubId);
            return View(stadium);
        }

        // GET: Stadia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stadia == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadia.FindAsync(id);
            if (stadium == null)
            {
                return NotFound();
            }
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubCoachName", stadium.ClubId);
            return View(stadium);
        }

        // POST: Stadia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StadiumId,StadiumLocation,StadiumCapacity,StadiumEstablismentDate,ClubId")] Stadium stadium)
        {
            if (id != stadium.StadiumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stadium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StadiumExists(stadium.StadiumId))
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
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubCoachName", stadium.ClubId);
            return View(stadium);
        }

        // GET: Stadia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stadia == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadia
                .Include(s => s.Club)
                .FirstOrDefaultAsync(m => m.StadiumId == id);
            if (stadium == null)
            {
                return NotFound();
            }

            return View(stadium);
        }

        // POST: Stadia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stadia == null)
            {
                return Problem("Entity set 'FootBallBdContext.Stadia'  is null.");
            }
            var stadium = await _context.Stadia.FindAsync(id);
            if (stadium != null)
            {
                _context.Stadia.Remove(stadium);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StadiumExists(int id)
        {
          return _context.Stadia.Any(e => e.StadiumId == id);
        }
    }
}
