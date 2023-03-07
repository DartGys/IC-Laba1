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
    public class MatchesController : Controller
    {
        private readonly FootBallBdContext _context;

        public MatchesController(FootBallBdContext context)
        {
            _context = context;
        }

        // GET: Matches
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Championships", "Index");

            ViewBag.ChampionshipId = id;  
            ViewBag.ChampionshipName = name;
            var matchesByChampionships = _context.Matches.Where(b => b.ChampionshipId == id).Include(b => b.Championship).Include(b => b.HostClub).Include(b => b.GuestClub).Include(b => b.Stadium);
            return View(await matchesByChampionships.ToListAsync());
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Matches == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.Championship)
                .Include(m => m.GuestClub)
                .Include(m => m.HostClub)
                .Include(m => m.Stadium)
                .FirstOrDefaultAsync(m => m.MatchId == id);
            if (match == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "ScoredGoals", new { id = match.MatchId}); ;
        }

        // GET: Matches/Create
        public IActionResult Create(int championshipId)
        {
            //ViewData["ChampionshipId"] = new SelectList(_context.Championships, "ChampionshipId", "ChampionshipCountry");
            ViewData["GuestClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName");
            ViewData["HostClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName");
            ViewData["StaidumId"] = new SelectList(_context.Stadia, "StadiumId", "StadiumLocation");
            ViewBag.ChampionshipId = championshipId;
            ViewBag.ChampionshipName = _context.Championships.Where(c => c.ChampionshipId == championshipId).FirstOrDefault().ChampionshipName;
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int championshipId,[Bind("MatchId,MatchDate,MatchDuration,StaidumId,HostClubId,GuestClubId,ChampionshipId")] Match match)
        {
            match.ChampionshipId = championshipId;
            if (ModelState.IsValid)
            {
                _context.Add(match);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Matches", new { id = championshipId, name = _context.Championships.Where(c => c.ChampionshipId == championshipId).FirstOrDefault().ChampionshipName});
            }
            //ViewData["ChampionshipId"] = new SelectList(_context.Championships, "ChampionshipId", "ChampionshipCountry", match.ChampionshipId);
            //ViewData["GuestClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubCoachName", match.GuestClubId);
            //ViewData["HostClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubCoachName", match.HostClubId);
            //ViewData["StaidumId"] = new SelectList(_context.Stadia, "StadiumId", "StadiumLocation", match.StaidumId);
            return RedirectToAction("Index", "Matches", new { id = championshipId, name = _context.Championships.Where(c => c.ChampionshipId == championshipId).FirstOrDefault().ChampionshipName });
        }

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Matches == null)
            {
                return NotFound();
            }

            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            }
            ViewData["ChampionshipId"] = new SelectList(_context.Championships, "ChampionshipId", "ChampionshipCountry", match.ChampionshipId);
            ViewData["GuestClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubCoachName", match.GuestClubId);
            ViewData["HostClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubCoachName", match.HostClubId);
            ViewData["StaidumId"] = new SelectList(_context.Stadia, "StadiumId", "StadiumLocation", match.StaidumId);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MatchId,MatchDate,MatchDuration,StaidumId,HostClubId,GuestClubId,ChampionshipId")] Match match)
        {
            if (id != match.MatchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.MatchId))
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
            ViewData["ChampionshipId"] = new SelectList(_context.Championships, "ChampionshipId", "ChampionshipCountry", match.ChampionshipId);
            ViewData["GuestClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubCoachName", match.GuestClubId);
            ViewData["HostClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubCoachName", match.HostClubId);
            ViewData["StaidumId"] = new SelectList(_context.Stadia, "StadiumId", "StadiumLocation", match.StaidumId);
            return View(match);
        }

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Matches == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.Championship)
                .Include(m => m.GuestClub)
                .Include(m => m.HostClub)
                .Include(m => m.Stadium)
                .FirstOrDefaultAsync(m => m.MatchId == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Matches == null)
            {
                return Problem("Entity set 'FootBallBdContext.Matches'  is null.");
            }
            var match = await _context.Matches.FindAsync(id);
            if (match != null)
            {
                _context.Matches.Remove(match);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchExists(int id)
        {
          return _context.Matches.Any(e => e.MatchId == id);
        }
    }
}
