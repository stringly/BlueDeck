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
    /// Controller that handles CRUD actions for the <see cref="DutyStatus"/> entity.
    /// </summary>
    /// <remarks>
    /// Actions in this controller are restricted by the <see cref="IsGlobalAdminRequirement"/>
    /// </remarks>
    /// <seealso cref="Controller" />
    [Authorize("IsGlobalAdmin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DutyStatusController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DutyStatusController"/> class.
        /// </summary>
        /// <param name="unit">The injected <see cref="IUnitOfWork"/> obtained from the services middleware.</param>
        public DutyStatusController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// Returns a List view of all of the <see cref="DutyStatus"/> entities.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DutyStatus/Index")]
        public IActionResult Index()
        {
            return View(unitOfWork.MemberDutyStatus.GetAll());
        }

        /// <summary>
        /// Returns a view that displays details of a <see cref="DutyStatus"/>
        /// </summary>
        /// <param name="id">The identity of the <see cref="DutyStatus"/></param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("DutyStatus/Details/{id:int}")]
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

        /// <summary>
        /// Returns a view that allows the user to create a new <see cref="DutyStatus"/> entity.
        /// </summary>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("DutyStatus/Create")]
        public IActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// Creates the specified duty status.
        /// </summary>
        /// <param name="dutyStatus">The form data, bound to a <see cref="DutyStatus"/> object.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("DutyStatus/Create")]
        public IActionResult Create([Bind("DutyStatusId,DutyStatusName,HasPolicePower,IsExceptionToNormalDuty,Abbreviation")] DutyStatus dutyStatus, string returnUrl)
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

        /// <summary>
        /// Returns a view that allows a user to edit an existing <see cref="DutyStatus"/> entity.
        /// </summary>
        /// <param name="id">The identity of the <see cref="DutyStatus"/> to be edited.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("DutyStatus/Edit/{id:int}")]
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

        /// <summary>
        /// Edits the <see cref="DutyStatus"/> with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="DutyStatus"/> entity being edited.</param>
        /// <param name="dutyStatus">The POSTed form data, which will be bound to a <see cref="DutyStatus"/> object.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("DutyStatus/Edit/{id:int}")]
        public IActionResult Edit(int id, [Bind("DutyStatusId,DutyStatusName,HasPolicePower,IsExceptionToNormalDuty,Abbreviation")] DutyStatus dutyStatus, string returnUrl)
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
        /// <summary>
        /// Retrieves the view to confirm the deletion of a Duty Status
        /// </summary>
        /// <param name="id">The DutyStatusId of the Duty Status to be deleted.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("DutyStatus/Delete/{id:int}")]
        public IActionResult Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (DutyStatusExists(id))
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(unitOfWork.MemberDutyStatus.GetDutyStatusWithMemberCount((Int32)id));
            }
            else
            {
                return NotFound();
            }            
        }

        /// <summary>
        /// Deletes the Duty Status with the given DutyStatusId.
        /// </summary>
        /// <param name="id">The DutyStatusId of the Duty Status to be deleted.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("DutyStatus/Delete/{id:int}")]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            DutyStatus toRemove = unitOfWork.MemberDutyStatus.GetDutyStatusWithMemberCount((Int32)id);
            if (toRemove != null && toRemove.Members.Count() == 0)
            {
                unitOfWork.MemberDutyStatus.Remove(toRemove);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Status successfully deleted.";
            }
            else
            {
                ViewBag.Status = "Warning!";
                ViewBag.Message = "You cannot delete a Duty Status with active Members.";
                ViewBag.ReturnUrl = returnUrl;
                return View(toRemove);
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
