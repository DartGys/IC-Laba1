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
    public class ScoredGoalsController : Controller
    {
        private readonly FootBallBdContext _context;

        public ScoredGoalsController(FootBallBdContext context)
        {
            _context = context;
        }

        // GET: ScoredGoals
        public async Task<IActionResult> Index(int? id)
        {

            if (id == null) return RedirectToAction("Matches", "Index");

            ViewBag.MatchId = id; 
            var matchesGoal = _context.ScoredGoals.Where(b => b.MatchId == id).Include(b => b.Match).Include(b => b.Player);
            return View(await matchesGoal.ToListAsync());
        }

        // GET: ScoredGoals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ScoredGoals == null)
            {
                return NotFound();
            }

            var scoredGoal = await _context.ScoredGoals
                .Include(s => s.Match)
                .Include(s => s.Player)
                .FirstOrDefaultAsync(m => m.ScoredGoalId == id);
            if (scoredGoal == null)
            {
                return NotFound();
            }

            return View(scoredGoal);
        }

        // GET: ScoredGoals/Create
        public IActionResult Create(int MatchId)
        {
            //ViewData["MatchId"] = new SelectList(_context.Matches, "MatchId", "MatchId");
            ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "PlayerName");
            ViewBag.MatchId = MatchId;
            return View();
        }

        // POST: ScoredGoals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int MatchId,[Bind("ScoredGoalId,ScoredMinute,PlayerId,MatchId")] ScoredGoal scoredGoal)
        {
            scoredGoal.MatchId = MatchId;
            if (ModelState.IsValid)
            {
                _context.Add(scoredGoal);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "ScoredGoals", new { id = MatchId, name = _context.Matches.Where(c => c.MatchId == MatchId).FirstOrDefault().MatchDate });
            }
            //ViewData["MatchId"] = new SelectList(_context.Matches, "MatchId", "MatchId", scoredGoal.MatchId);
            //ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "PlayerName", scoredGoal.PlayerId);
            return View(scoredGoal);
        }

        // GET: ScoredGoals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ScoredGoals == null)
            {
                return NotFound();
            }

            var scoredGoal = await _context.ScoredGoals.FindAsync(id);
            if (scoredGoal == null)
            {
                return NotFound();
            }
            ViewData["MatchId"] = new SelectList(_context.Matches, "MatchId", "MatchId", scoredGoal.MatchId);
            ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "PlayerName", scoredGoal.PlayerId);
            return View(scoredGoal);
        }

        // POST: ScoredGoals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScoredGoalId,ScoredMinute,PlayerId,MatchId")] ScoredGoal scoredGoal)
        {
            if (id != scoredGoal.ScoredGoalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scoredGoal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScoredGoalExists(scoredGoal.ScoredGoalId))
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
            ViewData["MatchId"] = new SelectList(_context.Matches, "MatchId", "MatchId", scoredGoal.MatchId);
            ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "PlayerName", scoredGoal.PlayerId);
            return View(scoredGoal);
        }

        // GET: ScoredGoals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ScoredGoals == null)
            {
                return NotFound();
            }

            var scoredGoal = await _context.ScoredGoals
                .Include(s => s.Match)
                .Include(s => s.Player)
                .FirstOrDefaultAsync(m => m.ScoredGoalId == id);
            if (scoredGoal == null)
            {
                return NotFound();
            }

            return View(scoredGoal);
        }

        // POST: ScoredGoals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ScoredGoals == null)
            {
                return Problem("Entity set 'FootBallBdContext.ScoredGoals'  is null.");
            }
            var scoredGoal = await _context.ScoredGoals.FindAsync(id);
            if (scoredGoal != null)
            {
                _context.ScoredGoals.Remove(scoredGoal);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScoredGoalExists(int id)
        {
          return _context.ScoredGoals.Any(e => e.ScoredGoalId == id);
        }
    }
}
