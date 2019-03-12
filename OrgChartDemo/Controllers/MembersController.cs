using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Types;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.Controllers
{
    /// <summary>
    /// Controller for Member CRUD actions
    /// </summary>
    /// <seealso cref="Controller" />
    public class MembersController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembersController"/> class.
        /// </summary>
        /// <param name="unit">A Dependency-Injected IUnitOfWork object.<see cref="OrgChartDemo.Persistence.UnitOfWork"/>.</param>
        public MembersController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }
        
        /// <summary>
        /// GET: Members
        /// </summary>
        /// <remarks>        
        /// </remarks>
        /// <returns>An <see cref="IActionResult"/></returns>
        public IActionResult Index(string sortOrder, string searchString)
        {
            MemberIndexListViewModel vm = new MemberIndexListViewModel(unitOfWork.Members.GetMembersWithPositions().ToList());
            vm.CurrentSort = sortOrder;
            vm.MemberLastNameSort = String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
            vm.MemberFirstNameSort = sortOrder == "FirstName" ? "firstName_desc" : "FirstName";
            vm.IdNumberSort = sortOrder == "IDNumber" ? "idNumber_desc" : "IDNumber";
            vm.PositionNameSort = sortOrder == "PositionName" ? "positionName_desc" : "PositionName";
            vm.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                vm.Members = vm.Members
                    .Where(x => x.LastName.Contains(searchString) || x.FirstName.Contains(searchString) || x.PositionName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "lastName_desc":
                    vm.Members = vm.Members.OrderByDescending(x => x.LastName);
                    break;
                case "firstName_desc":
                    vm.Members = vm.Members.OrderByDescending(x => x.FirstName);
                    break;
                case "FirstName":
                    vm.Members = vm.Members.OrderBy(x => x.FirstName);
                    break;
                case "idNumber_desc":
                    vm.Members = vm.Members.OrderByDescending(x => x.IdNumber);
                    break;
                case "IDNumber":
                    vm.Members = vm.Members.OrderBy(x => x.IdNumber);
                    break;
                case "PositionName":
                    vm.Members = vm.Members.OrderBy(x => x.PositionName);
                    break;
                case "positionName_desc":
                    vm.Members = vm.Members.OrderByDescending(x => x.PositionName);
                    break;
                default:
                    vm.Members = vm.Members.OrderBy(x => x.LastName);
                    break;
            }
            ViewBag.Title = "BlueDeck Members Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            return View(vm);            
        }

        /// <summary>
        /// GET: Members/Details/5.
        /// </summary>
        /// <param name="id">The identifier for a Member.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = unitOfWork.Members.GetMemberWithDemographicsAndDutyStatus(Convert.ToInt32(id));
            if (member == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Member Details";
            return View(member);
        }

        /// <summary>
        /// GET: Members/Create.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/></returns>
        public IActionResult Create()
        {
            MemberAddEditViewModel vm = new MemberAddEditViewModel(new Member(),
                unitOfWork.Positions.GetAllPositionSelectListItems(),
                unitOfWork.MemberRanks.GetMemberRankSelectListItems(),
                unitOfWork.MemberGenders.GetMemberGenderSelectListItems(),
                unitOfWork.MemberRaces.GetMemberRaceSelectListItems(),
                unitOfWork.MemberDutyStatus.GetMemberDutyStatusSelectListItems(),
                unitOfWork.PhoneNumberTypes.GetPhoneNumberTypeSelectListItems());
            ViewBag.Title = "Create New Member";
            return View(vm);
        }

        /// <summary>
        /// POST: Members/Create.
        /// </summary>
        /// <param name="form">A <see cref="MemberAddEditViewModel"/> with certain fields bound on submit</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstName,LastName,MiddleName,MemberRank,DutyStatusId,MemberRace,MemberGender,PositionId,IdNumber,Email,ContactNumbers")] MemberAddEditViewModel form)
        {
            if (!ModelState.IsValid)
            {
                form.Positions = unitOfWork.Positions.GetAllPositionSelectListItems();
                form.RaceList = unitOfWork.MemberRaces.GetMemberRaceSelectListItems();
                form.RankList = unitOfWork.MemberRanks.GetMemberRankSelectListItems();
                form.GenderList = unitOfWork.MemberGenders.GetMemberGenderSelectListItems();
                form.DutyStatus = unitOfWork.MemberDutyStatus.GetMemberDutyStatusSelectListItems();
                form.PhoneNumberTypes = unitOfWork.PhoneNumberTypes.GetPhoneNumberTypeSelectListItems();
                ViewBag.Title = "Create Member - Corrections Required";
                ViewBag.Status = "Warning!";
                ViewBag.Message = "You must correct the fields indicated.";
                return View(form);
            }
            else
            {
                // TODO: Member addition checks? Duplicate Name/Badge Numbers?
                unitOfWork.Members.UpdateMember(form);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Member successfully created.";
                return RedirectToAction(nameof(Index));
            }            
        }
        
        /// <summary>
        /// Members/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [Authorize("CanEditUser")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Member member = unitOfWork.Members.GetMemberWithDemographicsAndDutyStatus(Convert.ToInt32(id));
            if (member == null)
            {
                return NotFound();
            }
            MemberAddEditViewModel vm = new MemberAddEditViewModel(member, 
                unitOfWork.Positions.GetAllPositionSelectListItems(), 
                unitOfWork.MemberRanks.GetMemberRankSelectListItems(),
                unitOfWork.MemberGenders.GetMemberGenderSelectListItems(),
                unitOfWork.MemberRaces.GetMemberRaceSelectListItems(),
                unitOfWork.MemberDutyStatus.GetMemberDutyStatusSelectListItems(),
                unitOfWork.PhoneNumberTypes.GetPhoneNumberTypeSelectListItems());
            ViewBag.Title = "Edit Member";
            return View(vm);
        }

        /// <summary>
        /// POST: Members/Edit/5
        /// </summary>
        /// <param name="id">The MemberId for the <see cref="Member"/> being edited</param>
        /// <param name="form">The <see cref="MemberAddEditViewModel"/> object to which the POSTed form is Bound</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("MemberId,FirstName,LastName,MiddleName,MemberRank,DutyStatusId,MemberGender,MemberRace,PositionId,IdNumber,Email,ContactNumbers")] MemberAddEditViewModel form)
        {

            if (!ModelState.IsValid)
            {
                // Model Validation failed, repopulate the ViewModel's List data and return the View
                form.Positions = unitOfWork.Positions.GetAllPositionSelectListItems();
                form.RaceList = unitOfWork.MemberRaces.GetMemberRaceSelectListItems();
                form.RankList = unitOfWork.MemberRanks.GetMemberRankSelectListItems();
                form.GenderList = unitOfWork.MemberGenders.GetMemberGenderSelectListItems();
                form.DutyStatus = unitOfWork.MemberDutyStatus.GetMemberDutyStatusSelectListItems();
                form.PhoneNumberTypes = unitOfWork.PhoneNumberTypes.GetPhoneNumberTypeSelectListItems();
                ViewBag.Title = "Edit Member - Corrections Required";
                ViewBag.Status = "Warning!";
                ViewBag.Message = "You must correct the fields indicated.";
                return View(form);                
            }
            else
            {
                try
                {
                    unitOfWork.Members.UpdateMember(form);
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
            TempData["Status"] = "Success!";
            TempData["Message"] = "Member successfully updated.";
            return RedirectToAction(nameof(Index));
            }
        }
                
        /// <summary>
        /// GET: Member/Delete/5
        /// </summary>
        /// <param name="id">The MemberId of the <see cref="Member"/> being deleted</param>
        /// <returns>An <see cref="IActionResult"/> that prompts the user to confirm the deletion of the Member with the given id</returns>
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // TODO: Handle deleting a Member and vacating a Position that is Manager of it's component 
            Member m = unitOfWork.Members.GetMemberWithDemographicsAndDutyStatus(Convert.ToInt32(id));
            if (m == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Confirm - Delete Member?";
            return View(m);
        }
        
        /// <summary>
        /// POST: Members/Delete/5
        /// </summary>
        /// <param name="id">The MemberId of the <see cref="Member"/> being deleted</param>
        /// <returns>An <see cref="IActionResult"/> that redirects to <see cref="MembersController.Index"/> on successful deletion of a Member.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {            
            unitOfWork.Members.Remove(id);
            unitOfWork.Complete();
            TempData["Status"] = "Success!";
            TempData["Message"] = "Member successfully deleted.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Determines if a Member exists with the provided MemberId .
        /// </summary>
        /// <param name="id">The MemberId of the <see cref="Member"/></param>
        /// <returns>True if a <see cref="Member"/> with the given id exists</returns>
        private bool MemberExists(int id)
        {
            return (unitOfWork.Members.SingleOrDefault(e => e.MemberId == id) != null);
        }

        /// <summary>
        /// Gets the member contact number view component.
        /// </summary>
        /// <remarks>
        /// This View Component is used to add Phone Numbers in the Members/Edit and Member/Edit Modal Views
        /// </remarks>
        /// <param name="memberId">The member identifier.</param>
        /// <returns></returns>
        public IActionResult GetMemberContactNumberViewComponent(int memberId)
        {
            Member m = unitOfWork.Members.GetMemberWithDemographicsAndDutyStatus(memberId);
            List<PhoneNumberTypeSelectListItem> phoneTypes = unitOfWork.PhoneNumberTypes.GetPhoneNumberTypeSelectListItems();
            MemberContactNumberViewComponentViewModel vm = new MemberContactNumberViewComponentViewModel(m, phoneTypes);
            return ViewComponent("MemberContactNumber", vm);
        }
    }
}
