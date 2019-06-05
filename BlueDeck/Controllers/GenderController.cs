using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Auth;

namespace BlueDeck.Controllers
{
    /// <summary>
    /// Controller that handles CRUD actions and views for the <see cref="Gender"/> entity.
    /// </summary>
    /// <remarks>
    /// Actions in this view are restricted by the <see cref="IsGlobalAdminRequirement"/>
    /// </remarks>
    /// <seealso cref="Controller" />
    [Authorize("IsGlobalAdmin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class GenderController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenderController"/> class.
        /// </summary>
        /// <param name="unit">The injected <see cref="IUnitOfWork"/> obtained from the services middleware.</param>
        public GenderController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// Returns a view that lists all current <see cref="Gender"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Gender/Index")]
        public IActionResult Index()
        {
            return View(unitOfWork.MemberGenders.GetAll());
        }

        /// <summary>
        /// Returns a view that shows details for a specific <see cref="Gender"/> entity.
        /// </summary>
        /// <param name="id">The identity of the <see cref="Gender"/>.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Gender/Details/{id:int}")]
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

        /// <summary>
        /// Retrieves the view that allows a user to create a new <see cref="Gender"/>
        /// </summary>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Gender/Create")]
        public IActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// Creates the specified gender.
        /// </summary>
        /// <param name="gender">The POSTed form data, bound to a <see cref="Gender"/> object.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Gender/Create")]
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

        /// <summary>
        /// Returns a view that allows a user to edit an existing <see cref="Gender"/>
        /// </summary>
        /// <param name="id">The identity of the <see cref="Gender"/> to be edited.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Gender/Edit/{id:int}")]
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

        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="Gender"/> being edited.</param>
        /// <param name="gender">The POSTed form data, bound to a <see cref="Gender"/> object.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Gender/Edit/{id:int}")]
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

        /// <summary>
        /// Retrieves a view to confirm the deletion of the <see cref="Gender"/> with the provided id.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="Gender"/> to be deleted.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Gender/Delete/{id:int}")]
        public IActionResult Delete(int? id, string returnUrl)
        {
            if (id == null || !GenderExists(id))
            {
                return NotFound();
            }
            else
            {
                var gender = unitOfWork.MemberGenders.GetGenderWithMembers((Int32)id);
                ViewBag.ReturnUrl = returnUrl;
                return View(gender);
            }
        }

        /// <summary>
        /// Deletes the <see cref="Gender"/> with the provided id.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="Gender"/> to be deleted.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Gender/Delete/{id:int}")]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            Gender toRemove = unitOfWork.MemberGenders.GetGenderWithMembers((Int32)id);
            if (toRemove != null && toRemove.Members.Count() == 0)
            {
                unitOfWork.MemberGenders.Remove(toRemove);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Gender successfully deleted.";
            }
            else
            {
                ViewBag.Status = "Warning!";
                ViewBag.Message = "You cannot delete a Gender with active Members";
                ViewBag.ReturnUrl = returnUrl;
                return View(toRemove);
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
