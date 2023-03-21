using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootBallWebLaba1.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

            return RedirectToAction("Index", "ScoredGoals", new { id = match.MatchId});
        }



        // GET: Matches/Create
        public IActionResult Create(int championshipId)
        {
            //ViewData["ChampionshipId"] = new SelectList(_context.Championships, "ChampionshipId", "ChampionshipCountry");
            ViewData["GuestClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName");
            ViewData["HostClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName");
            //ViewData["StaidumId"] = new SelectList(_context.Stadiums, "StadiumId", "StadiumLocation");
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
            var stadium = await _context.Stadiums.FirstAsync(s => s.ClubId == match.HostClubId);
            match.StaidumId = stadium.StadiumId;
            if (ModelState.IsValid)
            {
                if(match.HostClubId == match.GuestClubId)
                {
                    ModelState.AddModelError("MatchDuration", "В матчі беруть участь дві різні команди");
                    ViewData["ChampionshipId"] = new SelectList(_context.Championships, "ChampionshipId", "ChampionshipCountry", match.ChampionshipId);
                    ViewData["GuestClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.GuestClubId);
                    ViewData["HostClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.HostClubId);
                    ViewData["StaidumId"] = new SelectList(_context.Stadiums, "StadiumId", "StadiumLocation", match.StaidumId);
                    ViewBag.ChampionshipId = championshipId;
                    return View(match);
                }

                var guestClubPlayers = await _context.Players.FirstOrDefaultAsync(g => g.ClubId == match.GuestClubId);
                var hostClubPlayers = await _context.Players.FirstOrDefaultAsync(g => g.ClubId == match.HostClubId);


                if(hostClubPlayers == null || hostClubPlayers == null)
                {
                    ModelState.AddModelError("MatchDuration", "Зазначені команди не мають жодного гравця в складі");
                    ViewData["ChampionshipId"] = new SelectList(_context.Championships, "ChampionshipId", "ChampionshipCountry", match.ChampionshipId);
                    ViewData["GuestClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.GuestClubId);
                    ViewData["HostClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.HostClubId);
                    ViewData["StaidumId"] = new SelectList(_context.Stadiums, "StadiumId", "StadiumLocation", match.StaidumId);
                    ViewBag.ChampionshipId = championshipId;
                    return View(match);
                }

                var guestClub = await _context.Clubs.FirstOrDefaultAsync(g => g.ClubId == match.GuestClubId);
                var hostClub = await _context.Clubs.FirstOrDefaultAsync(g => g.ClubId == match.HostClubId);

                DateTime dateTime = DateTime.Now;

                if (guestClub.ClubEstablishmentDate > match.MatchDate || hostClub.ClubEstablishmentDate > match.MatchDate)
                {
                    ModelState.AddModelError("MatchDate", "Дата проведення матчу не може передувати даті створення команд");
                    ViewData["ChampionshipId"] = new SelectList(_context.Championships, "ChampionshipId", "ChampionshipCountry", match.ChampionshipId);
                    ViewData["GuestClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.GuestClubId);
                    ViewData["HostClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.HostClubId);
                    ViewData["StaidumId"] = new SelectList(_context.Stadiums, "StadiumId", "StadiumLocation", match.StaidumId);
                    ViewBag.ChampionshipId = championshipId;
                    return View(match);
                }

                if(match.MatchDate > dateTime)
                {
                    ModelState.AddModelError("MatchDate", "Дата проведення матчу не може бути назначена на майбутнє");
                    ViewData["ChampionshipId"] = new SelectList(_context.Championships, "ChampionshipId", "ChampionshipCountry", match.ChampionshipId);
                    ViewData["GuestClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.GuestClubId);
                    ViewData["HostClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.HostClubId);
                    ViewData["StaidumId"] = new SelectList(_context.Stadiums, "StadiumId", "StadiumLocation", match.StaidumId);
                    ViewBag.ChampionshipId = championshipId;
                    return View(match);
                }    

                var checkAnotherChamp = await _context.Matches.FirstOrDefaultAsync(c => c.ChampionshipId != match.ChampionshipId && (c.HostClubId == match.HostClubId || c.GuestClubId == match.HostClubId || c.HostClubId == match.GuestClubId || c.GuestClubId == match.GuestClubId));
                    
                if(checkAnotherChamp != null)
                {
                    ModelState.AddModelError("MatchDuration", "Вказана команда уже бере участь в іншому чемпіонаті");
                    ViewData["ChampionshipId"] = new SelectList(_context.Championships, "ChampionshipId", "ChampionshipCountry", match.ChampionshipId);
                    ViewData["GuestClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.GuestClubId);
                    ViewData["HostClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.HostClubId);
                    ViewData["StaidumId"] = new SelectList(_context.Stadiums, "StadiumId", "StadiumLocation", match.StaidumId);
                    ViewBag.ChampionshipId = championshipId;
                    return View(match);
                }

                _context.Add(match);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Matches", new { id = championshipId, name = _context.Championships.Where(c => c.ChampionshipId == championshipId).FirstOrDefault().ChampionshipName});
            }
            ViewData["ChampionshipId"] = new SelectList(_context.Championships, "ChampionshipId", "ChampionshipCountry", match.ChampionshipId);
            ViewData["GuestClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.GuestClubId);
            ViewData["HostClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.HostClubId);
            ViewData["StaidumId"] = new SelectList(_context.Stadiums, "StadiumId", "StadiumLocation", match.StaidumId);
            return View(match);
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
            //ViewData["ChampionshipId"] = new SelectList(_context.Championships, "ChampionshipId", "ChampionshipCountry", match.ChampionshipId);
            ViewData["GuestClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.GuestClubId);
            ViewData["HostClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.HostClubId);
            ViewData["StaidumId"] = new SelectList(_context.Stadiums, "StadiumId", "StadiumLocation", match.StaidumId);
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
                if (match.HostClubId == match.GuestClubId)
                {
                    ModelState.AddModelError("MatchDuration", "В матчі беруть участь дві різні команди");
                    ViewData["ChampionshipId"] = new SelectList(_context.Championships, "ChampionshipId", "ChampionshipCountry", match.ChampionshipId);
                    ViewData["GuestClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.GuestClubId);
                    ViewData["HostClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.HostClubId);
                    ViewData["StaidumId"] = new SelectList(_context.Stadiums, "StadiumId", "StadiumLocation", match.StaidumId);
                    ViewBag.ChampionshipId = match.ChampionshipId;
                    return View(match);
                }
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
            //ViewData["ChampionshipId"] = new SelectList(_context.Championships, "ChampionshipId", "ChampionshipCountry", match.ChampionshipId);
            ViewData["GuestClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.GuestClubId);
            ViewData["HostClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubName", match.HostClubId);
            ViewData["StaidumId"] = new SelectList(_context.Stadiums, "StadiumId", "StadiumLocation", match.StaidumId);
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
