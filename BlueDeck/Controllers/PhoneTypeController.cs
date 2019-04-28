using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;

namespace BlueDeck.Controllers
{
    [Authorize("IsGlobalAdmin")]
    public class PhoneTypeController : Controller
    {
        private IUnitOfWork unitOfWork;

        public PhoneTypeController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        // GET: PhoneType
        public IActionResult Index()
        {
            return View(unitOfWork.PhoneNumberTypes.GetAll());
        }

        // GET: PhoneType/Details/5
        public IActionResult Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phoneType = unitOfWork.PhoneNumberTypes.Find(x => x.PhoneNumberTypeId == id).FirstOrDefault();
                
            if (phoneType == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(phoneType);
        }

        // GET: PhoneType/Create
        public IActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: PhoneType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("PhoneNumberTypeId,PhoneNumberTypeName")] PhoneNumberType phoneType, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.PhoneNumberTypes.Add(phoneType);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Phone Number Type successfully created.";
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
            return View(phoneType);
        }

        // GET: PhoneType/Edit/5
        public IActionResult Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phoneType = unitOfWork.PhoneNumberTypes.Find(x => x.PhoneNumberTypeId == id).FirstOrDefault();
            if (phoneType == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(phoneType);
        }

        // POST: PhoneType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("PhoneNumberTypeId,PhoneNumberTypeName")] PhoneNumberType phoneType, string returnUrl)
        {
            if (id != phoneType.PhoneNumberTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    PhoneNumberType toEdit = unitOfWork.PhoneNumberTypes.Get(id);
                    toEdit.PhoneNumberTypeName = phoneType.PhoneNumberTypeName;
                    unitOfWork.Complete();
                    TempData["Status"] = "Success!";
                    TempData["Message"] = "Phone Number Type successfully updated.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhoneTypeExists(phoneType.PhoneNumberTypeId))
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
            return View(phoneType);
        }

        // GET: PhoneType/Delete/5
        public IActionResult Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phoneType = unitOfWork.PhoneNumberTypes.Find(m => m.PhoneNumberTypeId == id).FirstOrDefault();
            if (phoneType == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(phoneType);
        }

        // POST: PhoneType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            PhoneNumberType toRemove = unitOfWork.PhoneNumberTypes.Find(x => x.PhoneNumberTypeId == id).FirstOrDefault();
            if (toRemove != null)
            {
                unitOfWork.PhoneNumberTypes.Remove(toRemove);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Phone Type successfully deleted.";
            }            
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PhoneTypeExists(int? id)
        {
            return unitOfWork.PhoneNumberTypes.Find(e => e.PhoneNumberTypeId == id) != null;
        }
    }
}
