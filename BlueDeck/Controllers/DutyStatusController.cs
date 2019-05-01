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
    public class DutyStatusController : Controller
    {
        private IUnitOfWork unitOfWork;

        public DutyStatusController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        // GET: DutyStatus
        public IActionResult Index()
        {
            return View(unitOfWork.MemberDutyStatus.GetAll());
        }

        // GET: AppStatus/Details/5
        public IActionResult Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dutyStatus = unitOfWork.MemberDutyStatus.Find(x => x.DutyStatusId == id).FirstOrDefault();
                
            if (dutyStatus == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(dutyStatus);
        }

        // GET: AppStatus/Create
        public IActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: DutyStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DutyStatusId,DutyStatusName,HasPolicePower,Abbreviation")] DutyStatus dutyStatus, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.MemberDutyStatus.Add(dutyStatus);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Status successfully created.";
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
            return View(dutyStatus);
        }

        // GET: DutyStatus/Edit/5
        public IActionResult Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dutyStatus = unitOfWork.MemberDutyStatus.Find(x => x.DutyStatusId == id).FirstOrDefault();
            if (dutyStatus == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(dutyStatus);
        }

        // POST: AppStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("DutyStatusId,DutyStatusName,HasPolicePower,Abbreviation")] DutyStatus dutyStatus, string returnUrl)
        {
            if (id != dutyStatus.DutyStatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DutyStatus toEdit = unitOfWork.MemberDutyStatus.Get(id);
                    toEdit.DutyStatusName = dutyStatus.DutyStatusName;
                    toEdit.HasPolicePower = dutyStatus.HasPolicePower;
                    toEdit.Abbreviation = dutyStatus.Abbreviation;
                    unitOfWork.Complete();
                    TempData["Status"] = "Success!";
                    TempData["Message"] = "Status successfully updated.";
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DutyStatusExists(dutyStatus.DutyStatusId))
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
            return View(dutyStatus);
        }

        // GET: AppStatus/Delete/5
        public IActionResult Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dutyStatus = unitOfWork.MemberDutyStatus.Find(m => m.DutyStatusId == id).FirstOrDefault();
            if (dutyStatus == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(dutyStatus);
        }

        // POST: AppStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            DutyStatus toRemove = unitOfWork.MemberDutyStatus.Find(x => x.DutyStatusId == id).FirstOrDefault();
            if (toRemove != null)
            {
                unitOfWork.MemberDutyStatus.Remove(toRemove);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Status successfully deleted.";
            }            
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DutyStatusExists(int? id)
        {
            return unitOfWork.MemberDutyStatus.Find(e => e.DutyStatusId == id) != null;
        }
    }
}
