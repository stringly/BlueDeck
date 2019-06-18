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
    /// Controller that handles CRUD actions for the <see cref="Rank"/> entity.
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize("IsGlobalAdmin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RankController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="RankController"/> class.
        /// </summary>
        /// <param name="unit">The injected <see cref="IUnitOfWork"/> obtained from the services middleware.</param>
        public RankController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// GET: Rank/Index
        /// </summary>
        /// <remarks>
        /// Retrieves a view that lists all <see cref="Rank"/> entities in the database.
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("Rank/Index")]
        public IActionResult Index()
        {
            return View(unitOfWork.MemberRanks.GetAll());
        }

        /// <summary>
        /// GET: Rank/Details/5
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Rank/Details/{id:int}")]
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

        /// <summary>
        /// GET: Rank/Create
        /// </summary>
        /// <remarks>
        /// Retrieves the view that allows a user to create a new <see cref="Rank"/>
        /// </remarks>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Rank/Create")]
        public IActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// POST: Rank/Create
        /// </summary>
        /// <param name="rank">The form data, bound to a <see cref="Rank"/> object.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Rank/Create")]
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

        /// <summary>
        /// GET: Rank/Edit/5
        /// </summary>
        /// <remarks>
        /// Returns a view that allows a user to edit an existing <see cref="Rank"/>
        /// </remarks>
        /// <param name="id">The identifier of the <see cref="Rank"/> to be edited.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Rank/Edit/{id:int}")]
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

        /// <summary>
        /// POST: Rank/Edit/5
        /// </summary>
        /// <remarks>
        /// Creates a new <see cref="Rank"/> from POSTed form data.
        /// </remarks>
        /// <param name="id">The indentifier of the <see cref="Rank"/> being edited.</param>
        /// <param name="rank">The POSTed form data, bound to a <see cref="Rank"/> object.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Rank/Edit/{id:int}")]
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

        /// <summary>
        /// GET: Rank/Delete/5
        /// </summary>
        /// <param name="id">The identifier of the <see cref="Rank"/> being edited.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Rank/Delete/{id:int}")]
        public IActionResult Delete(int? id, string returnUrl)
        {
            if (id == null || !RankExists(id))
            {
                return NotFound();
            }
            else
            {
                var race = unitOfWork.MemberRanks.GetRankWithMembers((Int32)id);
                ViewBag.ReturnUrl = returnUrl;
                return View(race);
            }
        }

        /// <summary>
        /// POST: Rank/Delete/5
        /// </summary>
        /// <param name="id">The identifier of the <see cref="Rank"/> being deleted.</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Rank/Delete/{id:int}")]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            Rank toRemove = unitOfWork.MemberRanks.GetRankWithMembers((Int32)id);
            if (toRemove != null && toRemove.Members.Count() == 0)
            {
                unitOfWork.MemberRanks.Remove(toRemove);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Rank successfully deleted.";
            }
            else
            {
                ViewBag.Status = "Warning";
                ViewBag.Message = "You cannot delete a Rank with active members.";
                ViewBag.ReturnUrl = returnUrl;
                return View(toRemove);
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