using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootBallWebLaba1.Models;
using System.Numerics;

namespace FootBallWebLaba1.Controllers
{
    public class ClubsController : Controller
    {
        private readonly FootBallBdContext _context;

        public ClubsController(FootBallBdContext context)
        {
            _context = context;
        }

        // GET: Clubs
        public async Task<IActionResult> Index()
        {
              return View(await _context.Clubs.ToListAsync());
        }

        // GET: Clubs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clubs == null)
            {
                return NotFound();
            }

            var club = await _context.Clubs
                .FirstOrDefaultAsync(m => m.ClubId == id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        public async Task<IActionResult> PlayersList(int? id)
        {
            if (id == null || _context.Clubs == null)
            {
                return NotFound();
            }

            var club = await _context.Clubs
                .FirstOrDefaultAsync(m => m.ClubId == id);
            if (club == null)
            {
                return NotFound();
            }

            return RedirectToAction("IndexPlayers", "Players", new { id = club.ClubId, name = club.ClubId });
        }

        // GET: Clubs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clubs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClubId,ClubName,ClubOrigin,ClubPlayerQuantity,ClubCoachName,ClubEstablishmentDate")] Club club)
        {
            club.ClubPlayerQuantity = 0;
            if (ModelState.IsValid)
            {
                var clubName = _context.Clubs.FirstOrDefault(c => c.ClubName == club.ClubName);
                var clubCoach = _context.Clubs.FirstOrDefault(c => c.ClubCoachName == club.ClubCoachName);

                DateTime curDate = DateTime.Now;
                DateTime clubDate = club.ClubEstablishmentDate;

                if(clubDate > curDate)
                {
                    ModelState.AddModelError("ClubEstablishmentDate", "Дата створення клубу не відповідає дійсності");
                    return View(club);
                }

                if(clubCoach != null)
                {
                    ModelState.AddModelError("ClubCoachName", "Цей тренер уже тренує іншу команду");
                    return View(club);
                }

                if (clubName != null)
                {
                    ModelState.AddModelError("ClubName", "Команда з такою назвою уже існує");
                    return View(club);
                }

                _context.Add(club);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Stadiums", new { clubId = club.ClubId});
            }
            return View(club);
        }

        // GET: Clubs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clubs == null)
            {
                return NotFound();
            }

            var club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                return NotFound();
            }
            return View(club);
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClubId,ClubName,ClubOrigin,ClubPlayerQuantity,ClubCoachName,ClubEstablishmentDate")] Club club)
        {
            if (id != club.ClubId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var clubName = _context.Clubs.FirstOrDefault(c => c.ClubName == club.ClubName && c.ClubId != club.ClubId);
                var clubCoach = _context.Clubs.FirstOrDefault(c => c.ClubCoachName == club.ClubCoachName && c.ClubId != club.ClubId);

                if (clubCoach != null)
                {
                    ModelState.AddModelError("ClubCoachName", "Цей тренер уже тренує іншу команду");
                    return View(club);
                }

                if (clubName != null)
                {
                    ModelState.AddModelError("ClubName", "Команда з такою назвою уже існує");
                    return View(club);
                }

                try
                {
                    _context.Update(club);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubExists(club.ClubId))
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
            return View(club);
        }

        // GET: Clubs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clubs == null)
            {
                return NotFound();
            }

            var club = await _context.Clubs
                .FirstOrDefaultAsync(m => m.ClubId == id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clubs == null)
            {
                return Problem("Entity set 'FootBallBdContext.Clubs'  is null.");
            }

            var club = await _context.Clubs
                .Include(c => c.Players)
                .Include(c => c.Stadiums)
                .FirstOrDefaultAsync(m => m.ClubId == id);

            var match = await _context.Matches
                .Where(m => m.HostClubId == id || m.GuestClubId == id)
                .Include(m => m.ScoredGoals)
                .FirstOrDefaultAsync();


            if (match != null)
            {
                foreach (var s in match.ScoredGoals)
                    _context.Remove(s);
                _context.Matches.Remove(match);
            }


            if (club != null)
            {
                foreach (var s in club.Stadiums)
                    _context.Remove(s);

                foreach (var p in club.Players)
                    _context.Remove(p);

                _context.Clubs.Remove(club);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ClubExists(int id)
        {
          return _context.Clubs.Any(e => e.ClubId == id);
        }
    }
}
