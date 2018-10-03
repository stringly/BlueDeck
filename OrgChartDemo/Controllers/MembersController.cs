using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Types;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.Controllers
{
    /// <summary>
    /// Controller for Member CRUD actions
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    public class MembersController : Controller
    {
        private IUnitOfWork unitOfWork;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Controllers.MembersController"/> class.
        /// </summary>
        /// <param name="unit"><see cref="T:OrgChartDemo.Persistence.UnitOfWork"/>.</param>
        public MembersController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }
        
        /// <summary>
        /// GET: Members
        /// </summary>
        /// <remarks>
        /// This View requires an <see cref="T:IEnumerable{T}"/> list of <see cref="T:OrgChartDemo.Models.ViewModels.PositionWithMemberCountItem"/>
        /// </remarks>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Index()
        {
            return View(unitOfWork.Members.GetAll());
        }

        /// <summary>
        /// GET: Members/Details/5.
        /// </summary>
        /// <param name="id">The identifier for a Member.</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = unitOfWork.Members.SingleOrDefault(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        /// <summary>
        /// GET: Members/Create.
        /// </summary>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Create()
        {
            MemberWithPositionListViewModel vm = new MemberWithPositionListViewModel(new Member(),
                unitOfWork.Positions.GetAll().ToList(),
                unitOfWork.MemberRanks.GetMemberRankSelectListItems());
            return View(vm);
        }

        /// <summary>
        /// POST: Members/Create.
        /// </summary>
        /// <param name="form">A <see cref="T:OrgChartDemo.Models.ViewModels.MemberWithPositionListViewModel"/> with certain fields bound on submit</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstName,LastName,MiddleName,MemberRankId,PositionId,IdNumber,Email")] MemberWithPositionListViewModel form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                // TODO: Member addition checks? Duplicate Name/Badge Numbers?
                Member m = new Member
                {
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    MiddleName = form.MiddleName,
                    Rank = unitOfWork.MemberRanks.SingleOrDefault(x => x.RankId == form.MemberRank),
                    Position = unitOfWork.Positions.SingleOrDefault(x => x.PositionId == form.PositionId),
                    IdNumber = form.IdNumber,
                    Email = form.Email
                };
                unitOfWork.Members.Add(m);
                unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }            
        }


        /// <summary>
        /// Members/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Member member = unitOfWork.Members.SingleOrDefault(x => x.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }
            MemberWithPositionListViewModel vm = new MemberWithPositionListViewModel(member, unitOfWork.Positions.GetAll().ToList(), unitOfWork.MemberRanks.GetMemberRankSelectListItems());
            return View(vm);
        }

        /// <summary>
        /// POST: Members/Edit/5
        /// </summary>
        /// <param name="id">The MemberId for the <see cref="T:OrgChartDemo.Models.Member"/> being edited</param>
        /// <param name="form">The <see cref="T:OrgChartDemo.Models.ViewModels.MemberWithPositionListViewModel"/> object to which the POSTed form is Bound</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("FirstName,LastName,MiddleName,MemberRankId,PositionId,IdNumber,Email")] MemberWithPositionListViewModel form)
        {
            Member m = unitOfWork.Members.SingleOrDefault(x => x.MemberId == id);
            Position targetPosition = unitOfWork.Positions.SingleOrDefault(x => x.PositionId == form.PositionId);
            MemberRank r = unitOfWork.MemberRanks.SingleOrDefault(x => x.RankId == form.MemberRank);

            if (id != form.MemberId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    m.FirstName = form.FirstName;
                    m.LastName = form.LastName;
                    m.MiddleName = form.MiddleName;
                    m.IdNumber = form.IdNumber;
                    m.Email = form.Email;
                    m.Rank = r;
                    unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
        }

        
        /// <summary>
        /// GET: Member/Delete/5
        /// </summary>
        /// <param name="id">The MemberId of the <see cref="T:OrgChartDemo.Models.Member"/> being deleted</param>
        /// <returns>An <see cref="T:IActionResult"/> that prompts the user to confirm the deletion of the Member with the given id</returns>
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // TODO: Handle deleting a Member and vacating a Position that is Manager of it's component 
            Member m = unitOfWork.Members.SingleOrDefault(x => x.MemberId == id);
            if (m == null)
            {
                return NotFound();
            }

            return View(m);
        }

        
        /// <summary>
        /// POST: Members/Delete/5
        /// </summary>
        /// <param name="id">The MemberId of the <see cref="T:OrgChartDemo.Models.Member"/> being deleted</param>
        /// <returns>An <see cref="T:IActionResult"/> that redirects to <see cref="T:OrgChartDemo.Controllers.MembersController.Index"/> on successful deletion of a Member.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Member m = unitOfWork.Members.Get(id);
            unitOfWork.Members.Remove(m);
            unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Determines if a Member exists with the provided MemberId .
        /// </summary>
        /// <param name="id">The MemberId of the <see cref="T:OrgChartDemo.Models.Member"/></param>
        /// <returns>True if a <see cref="T:OrgChartDemo.Models.Member"/> with the given id exists</returns>
        private bool MemberExists(int id)
        {
            return (unitOfWork.Members.SingleOrDefault(e => e.MemberId == id) != null);
        }
    }
}
