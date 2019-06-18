using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Enums;

namespace BlueDeck.Controllers
{
    /// <summary>
    /// Controller that handles views for CRUD function for the <see cref="PhoneNumberType"/> entity.
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize("IsGlobalAdmin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PhoneTypeController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneTypeController"/> class.
        /// </summary>
        /// <param name="unit">An injected <see cref="IUnitOfWork"/> obtained from the services middleware.</param>
        public PhoneTypeController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// GET: PhoneType/Index
        /// </summary>
        /// <remarks>
        /// This view shows a list of all <see cref="PhoneNumberType"/> entities in the database.
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("PhoneType/Index")]
        public IActionResult Index()
        {
            return View(unitOfWork.PhoneNumberTypes.GetAll());
        }

        /// <summary>
        /// GET: PhoneType/Details/5
        /// </summary>
        /// <remarks>
        /// This view displays detailed information for a <see cref="PhoneNumberType"/> entity.
        /// </remarks>
        /// <param name="id">The identifier of the <see cref="PhoneNumberType"/>.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("PhoneType/Details/{id:int}")]
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

        /// <summary>
        /// GET: PhoneType/Create
        /// </summary>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("PhoneType/Create")]
        public IActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// POST: PhoneType/Create
        /// </summary>
        /// <param name="phoneType">A POSTed form object bound to a <see cref="PhoneNumberType"/> object.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("PhoneType/Create")]
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

        /// <summary>
        /// GET: PhoneType/Edit
        /// </summary>
        /// <param name="id">The identity of the <see cref="PhoneNumberType"/> entity.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("PhoneType/Edit/{id:int}")]
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

        /// <summary>
        /// POST: PhoneType/Edit/5
        /// </summary>
        /// <param name="id">The identity of the <see cref="PhoneNumberType"/> entity being edited.</param>
        /// <param name="phoneType">A POSTed form object, bound to a <see cref="PhoneNumberType"/> object.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("PhoneType/Edit/{id:int}")]
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

        /// <summary>
        /// GET: PhoneType/Delete/5
        /// </summary>
        /// <param name="id">The identity of the <see cref="PhoneNumberType"/> being edited.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("PhoneType/Delete/{id:int}")]
        public IActionResult Delete(int? id, string returnUrl)
        {
            if (id == null || !PhoneTypeExists(id))
            {
                return NotFound();
            }
            else
            {
                var phoneType = unitOfWork.PhoneNumberTypes.GetPhoneNumberTypeWithPhoneNumbers((Int32)id);
                ViewBag.ReturnUrl = returnUrl;
                return View(phoneType);
            }            
        }

        /// <summary>
        /// POST: PhoneType/Delete/5
        /// </summary>
        /// <param name="id">The identity of the <see cref="PhoneNumberType"/> entity being edited.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("PhoneType/Delete/{id:int}")]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            PhoneNumberType toRemove = unitOfWork.PhoneNumberTypes.GetPhoneNumberTypeWithPhoneNumbers((Int32)id);
            if (toRemove != null && toRemove.ContactNumbers.Count() == 0)
            {
                unitOfWork.PhoneNumberTypes.Remove(toRemove);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Phone Type successfully deleted.";
            }
            else
            {
                ViewBag.Status = "Warning!";
                ViewBag.Message = "You cannot delete a Phone Number Type with active Phone Numbers.";
                ViewBag.ReturnUrl = returnUrl;
                return View(toRemove);
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
