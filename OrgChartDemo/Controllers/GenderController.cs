using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;

namespace OrgChartDemo.Controllers
{
    [Authorize("IsGlobalAdmin")]
    public class GenderController : Controller
    {
        private IUnitOfWork unitOfWork;

        public GenderController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        // GET: Gender
        public IActionResult Index()
        {
            return View(unitOfWork.MemberGenders.GetAll());
        }

        // GET: Gender/Details/5
        public IActionResult Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gender = unitOfWork.MemberGenders.Find(x => x.GenderId == id).FirstOrDefault();
                
            if (gender == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(gender);
        }

        // GET: Gender/Create
        public IActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Gender/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("GenderId,GenderFullName,Abbreviation")] Gender gender, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.MemberGenders.Add(gender);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Gender successfully created.";
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
            return View(gender);
        }

        // GET: Gender/Edit/5
        public IActionResult Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gender = unitOfWork.MemberGenders.Find(x => x.GenderId == id).FirstOrDefault();
            if (gender == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(gender);
        }

        // POST: Gender/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("GenderId,GenderFullName,Abbreviation")] Gender gender, string returnUrl)
        {
            if (id != gender.GenderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Gender toEdit = unitOfWork.MemberGenders.Get(id);
                    toEdit.GenderFullName = gender.GenderFullName;
                    toEdit.Abbreviation = gender.Abbreviation;
                    unitOfWork.Complete();
                    TempData["Status"] = "Success!";
                    TempData["Message"] = "Gender successfully updated.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenderExists(gender.GenderId))
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
            return View(gender);
        }

        // GET: Gender/Delete/5
        public IActionResult Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gender = unitOfWork.MemberGenders.Find(m => m.GenderId == id).FirstOrDefault();
            if (gender == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(gender);
        }

        // POST: Gender/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            Gender toRemove = unitOfWork.MemberGenders.Find(x => x.GenderId == id).FirstOrDefault();
            if (toRemove != null)
            {
                unitOfWork.MemberGenders.Remove(toRemove);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Gender successfully deleted.";
            }            
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GenderExists(int? id)
        {
            return unitOfWork.MemberGenders.Find(e => e.GenderId == id) != null;
        }
    }
}
