using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;

namespace BlueDeck.Controllers
{
    [Authorize("IsGlobalAdmin")]
    public class AppStatusController : Controller
    {
        private IUnitOfWork unitOfWork;

        public AppStatusController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        // GET: AppStatus
        public IActionResult Index()
        {
            return View(unitOfWork.AppStatuses.GetAll());
        }

        // GET: AppStatus/Details/5
        public IActionResult Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appStatus = unitOfWork.AppStatuses.Find(x => x.AppStatusId == id).FirstOrDefault();
                
            if (appStatus == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(appStatus);
        }

        // GET: AppStatus/Create
        public IActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: AppStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("AppStatusId,StatusName")] AppStatus appStatus, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.AppStatuses.Add(appStatus);
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
            return View(appStatus);
        }

        // GET: AppStatus/Edit/5
        public IActionResult Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appStatus = unitOfWork.AppStatuses.Find(x => x.AppStatusId == id).FirstOrDefault();
            if (appStatus == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(appStatus);
        }

        // POST: AppStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("AppStatusId,StatusName")] AppStatus appStatus, string returnUrl)
        {
            if (id != appStatus.AppStatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    AppStatus toEdit = unitOfWork.AppStatuses.Get(id);
                    toEdit.StatusName = appStatus.StatusName;
                    unitOfWork.Complete();
                    TempData["Status"] = "Success!";
                    TempData["Message"] = "Status successfully updated.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppStatusExists(appStatus.AppStatusId))
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
            return View(appStatus);
        }

        // GET: AppStatus/Delete/5
        public IActionResult Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appStatus = unitOfWork.AppStatuses.Find(m => m.AppStatusId == id).FirstOrDefault();
            if (appStatus == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(appStatus);
        }

        // POST: AppStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            AppStatus toRemove = unitOfWork.AppStatuses.Find(x => x.AppStatusId == id).FirstOrDefault();
            if (toRemove != null)
            {
                unitOfWork.AppStatuses.Remove(toRemove);
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

        private bool AppStatusExists(int? id)
        {
            return unitOfWork.AppStatuses.Find(e => e.AppStatusId == id) != null;
        }
    }
}
