using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;

namespace BlueDeck.Controllers
{
    [Authorize("IsGlobalAdmin")]
    public class RankController : Controller
    {
        private IUnitOfWork unitOfWork;

        public RankController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        // GET: Rank
        public IActionResult Index()
        {
            return View(unitOfWork.MemberRanks.GetAll());
        }

        // GET: Rank/Details/5
        public IActionResult Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rank = unitOfWork.MemberRanks.Find(x => x.RankId == id).FirstOrDefault();
                
            if (rank == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(rank);
        }

        // GET: Rank/Create
        public IActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Rank/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("RankId,RankFullName,RankShort,PayGrade,IsSworn")] Rank rank, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.MemberRanks.Add(rank);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Rank successfully created.";
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
            return View(rank);
        }

        // GET: Rank/Edit/5
        public IActionResult Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rank = unitOfWork.MemberRanks.Find(x => x.RankId == id).FirstOrDefault();
            if (rank == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(rank);
        }

        // POST: Rank/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("RankId,RankFullName,RankShort,PayGrade,IsSworn")] Rank rank, string returnUrl)
        {
            if (id != rank.RankId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Rank toEdit = unitOfWork.MemberRanks.Get(id);
                    toEdit.RankFullName = rank.RankFullName;
                    toEdit.RankShort = rank.RankShort;
                    toEdit.PayGrade = rank.PayGrade;
                    toEdit.IsSworn = rank.IsSworn;
                    unitOfWork.Complete();
                    TempData["Status"] = "Success!";
                    TempData["Message"] = "Rank successfully updated.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RankExists(rank.RankId))
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
            return View(rank);
        }

        // GET: Rank/Delete/5
        public IActionResult Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = unitOfWork.MemberRanks.Find(m => m.RankId == id).FirstOrDefault();
            if (race == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(race);
        }

        // POST: Rank/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            Rank toRemove = unitOfWork.MemberRanks.Find(x => x.RankId == id).FirstOrDefault();
            if (toRemove != null)
            {
                unitOfWork.MemberRanks.Remove(toRemove);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Rank successfully deleted.";
            }            
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RankExists(int? id)
        {
            return unitOfWork.MemberRanks.Find(e => e.RankId == id) != null;
        }
    }
}