using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;

namespace BlueDeck.Controllers
{
    /// <summary>
    /// Controller that handles CRUD actions for the <see cref="Race"/> entity.
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize("IsGlobalAdmin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RaceController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="RaceController"/> class.
        /// </summary>
        /// <param name="unit">The injected <see cref="IUnitOfWork"/> obtained from the services middleware.</param>
        public RaceController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// GET: Race/Index
        /// </summary>
        /// <remarks>
        /// Retrieves a view that lists all <see cref="Race"/> entities in the database.
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("Race/Index")]
        public IActionResult Index()
        {
            return View(unitOfWork.MemberRaces.GetAll());
        }

        /// <summary>
        /// GET: Race/Details/5
        /// </summary>
        /// <remarks>
        /// Retrieves a view that shows details for a <see cref="Race"/>
        /// </remarks>
        /// <param name="id">The identifier of the <see cref="Race"/>.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Race/Details/{id:int}")]
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

        /// <summary>
        /// GET: Race/Create
        /// </summary>
        /// <remarks>
        /// Retrieves a view that allows a user to create a new <see cref="Race"/>
        /// </remarks>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Race/Create")]
        public IActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// POST: Race/Create
        /// </summary>
        /// <remarks>
        /// Creates a new <see cref="Race"/> from the POSTed form data.
        /// </remarks>
        /// <param name="race">The POSTed form data, bound to <see cref="Race"/></param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Race/Create")]
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

        /// <summary>
        /// GET: Race/Edit/5
        /// </summary>
        /// <remarks>
        /// Retrieves a view that allows a user to edit a <see cref="Race"/> entity.
        /// </remarks>
        /// <param name="id">The identifier if the <see cref="Race"/> being edited.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Race/Edit/{id:int}")]
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

        /// <summary>
        /// POST: Race/Edit/5
        /// </summary>
        /// <param name="id">The identifier of the <see cref="Race"/> being edited.</param>
        /// <param name="race">The POSTed form data, bound to a <see cref="Race"/> object.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Race/Edit/{id:int}")]
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

        /// <summary>
        /// GET: Race/Delete/5
        /// </summary>
        /// <param name="id">The identifier of the <see cref="Race"/> to delete.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Race/Delete/{id:int}")]
        public IActionResult Delete(int? id, string returnUrl)
        {
            if (id == null || !RaceExists(id))
            {
                return NotFound();
            }
            else
            {
                var race = unitOfWork.MemberRaces.GetRaceWithMembers((Int32)id);
                ViewBag.ReturnUrl = returnUrl;
                return View(race);
            }
        }

        /// <summary>
        /// POST: Race/Delete/5
        /// </summary>
        /// <param name="id">The identifier of the <see cref="Race"/> being deleted.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Race/Delete/{id:int}")]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            Race toRemove = unitOfWork.MemberRaces.GetRaceWithMembers((Int32)id);
            if (toRemove != null && toRemove.Members.Count() == 0)
            {
                unitOfWork.MemberRaces.Remove(toRemove);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Race successfully deleted.";
            }
            else
            {
                ViewBag.Status = "Warning!";
                ViewBag.Message = "You cannot delete a Race with current members.";
                ViewBag.ReturnUrl = returnUrl;
                return View(toRemove);
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
