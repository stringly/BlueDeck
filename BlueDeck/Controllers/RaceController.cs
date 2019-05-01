using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;

namespace BlueDeck.Controllers
{
    [Authorize("IsGlobalAdmin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RaceController : Controller
    {
        private IUnitOfWork unitOfWork;

        public RaceController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        // GET: Race
        public IActionResult Index()
        {
            return View(unitOfWork.MemberRaces.GetAll());
        }

        // GET: Race/Details/5
        public IActionResult Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = unitOfWork.MemberRaces.Find(x => x.MemberRaceId == id).FirstOrDefault();
                
            if (race == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(race);
        }

        // GET: Race/Create
        public IActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Race/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("MemberRaceId,MemberRaceFullName,Abbreviation")] Race race, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.MemberRaces.Add(race);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Race successfully created.";
                if (!String.IsNullOrEmpty(returnUrl))
                {                    
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }                
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(race);
        }

        // GET: Race/Edit/5
        public IActionResult Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = unitOfWork.MemberRaces.Find(x => x.MemberRaceId == id).FirstOrDefault();
            if (race == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(race);
        }

        // POST: Race/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("MemberRaceId,MemberRaceFullName,Abbreviation")] Race race, string returnUrl)
        {
            if (id != race.MemberRaceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Race toEdit = unitOfWork.MemberRaces.Get(id);
                    toEdit.MemberRaceFullName = race.MemberRaceFullName;
                    toEdit.Abbreviation = race.Abbreviation;
                    unitOfWork.Complete();
                    TempData["Status"] = "Success!";
                    TempData["Message"] = "Race successfully updated.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceExists(race.MemberRaceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(race);
        }

        // GET: Race/Delete/5
        public IActionResult Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = unitOfWork.MemberRaces.Find(m => m.MemberRaceId == id).FirstOrDefault();
            if (race == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(race);
        }

        // POST: Race/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            Race toRemove = unitOfWork.MemberRaces.Find(x => x.MemberRaceId == id).FirstOrDefault();
            if (toRemove != null)
            {
                unitOfWork.MemberRaces.Remove(toRemove);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Race successfully deleted.";
            }            
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RaceExists(int? id)
        {
            return unitOfWork.MemberRaces.Find(e => e.MemberRaceId == id) != null;
        }
    }
}
