using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;

namespace BlueDeck.Controllers
{
    /// <summary>
    /// Handles Create, Read, Update, and Delete functionality for the Application Status Enumeration.
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize("IsGlobalAdmin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AppStatusController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppStatusController"/> class.
        /// </summary>
        /// <param name="unit">The Injected <see cref="IUnitOfWork"/> obtained from the services middleware.</param>
        public AppStatusController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// Returns a view that lists all <see cref="AppStatus"/> in the database.
        /// </summary>
        /// <returns>The Views/Admin/Index <see cref="ViewResult"/></returns>
        [HttpGet]
        [Route("AppStatus/Index")]
        public IActionResult Index()
        {
            return View(unitOfWork.AppStatuses.GetAll());
        }

        /// <summary>
        /// Shows details for a specific <see cref="AppStatus"/>
        /// </summary>
        /// <param name="id">The AppStatusId of the <see cref="AppStatus"/>.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("AppStatus/Details/{id:int}")]
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

        /// <summary>
        /// Returns a <see cref="ViewResult"/> that allows creation of a new <see cref="AppStatus"/>.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("AppStatus/Create")]
        public IActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// Creates the specified application status.
        /// </summary>
        /// <param name="appStatus">The application status.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AppStatus/Create")]
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

        /// <summary>
        /// Edits the <see cref="AppStatus"/> with specified identifier.
        /// </summary>
        /// <param name="id">The AppStatusId of the <see cref="AppStatus"/> to edit.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("AppStatus/Edit/{id:int}")]
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

        /// <summary>
        /// Edits the <see cref="AppStatus"/> with the given AppStatusId.
        /// </summary>
        /// <param name="id">The identifier of the AppStatus.</param>
        /// <param name="appStatus">A POSTed form that binds to a <see cref="AppStatus"/>.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AppStatus/Edit/{id:int}")]
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

        /// <summary>
        /// Deletes the <see cref="AppStatus"/> with specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="AppStatus"/> to delete.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>The Views/AppStatus/Delete <see cref="ViewResult"/></returns>
        [HttpGet]
        [Route("AppStatus/Delete/{id:int}")]
        public IActionResult Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (AppStatusExists(id))
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(unitOfWork.AppStatuses.GetAppStatusWithMemberCount((Int32)id));
            }
            else {
                return NotFound();
            }
        }

        /// <summary>
        /// Confirms the deletion of an <see cref="AppStatus"/>.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="AppStatus"/>.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("AppStatus/Delete/{id:int}")]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            AppStatus toRemove = unitOfWork.AppStatuses.GetAppStatusWithMemberCount((Int32)id);
            if (toRemove != null && toRemove.Members.Count() == 0)
            {
                unitOfWork.AppStatuses.Remove(toRemove);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Status successfully deleted.";
            }
            else
            {
                ViewBag.Status = "Warning!";
                ViewBag.Message = "You cannot delete an Application Status with active Members.";
                ViewBag.ReturnUrl = returnUrl;
                return View(unitOfWork.AppStatuses.GetAppStatusWithMemberCount((Int32)id));
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
