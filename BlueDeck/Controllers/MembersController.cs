using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Types;
using BlueDeck.Models.ViewModels;
using BlueDeck.Models.Repositories;

namespace BlueDeck.Controllers
{
    /// <summary>
    /// Controller for Member CRUD actions
    /// </summary>
    /// <seealso cref="Controller" />
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MembersController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Property that determines the page length of List views returned from this controller.
        /// </summary>
        public int PageSize = 25;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembersController"/> class.
        /// </summary>
        /// <param name="unit">A Dependency-Injected IUnitOfWork object.<see cref="IUnitOfWork"/>.</param>
        public MembersController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// GET: The Members/Index view.
        /// </summary>
        /// <remarks>
        /// This method can return a paginated list of members with an optional sort order or search string.
        /// </remarks>
        /// <param name="sortOrder">
        /// An optional String parameter that will control the order of the result set. The options are:
        /// Last Name: (Ascending: Default)/"lastName_desc"
        /// First Name: "FirstName"/"firstName_desc"
        /// ID Number: "IDNumber"/"idNumber_desc"
        /// Position Name: "PositionName"/"positionName_desc"
        /// </param>
        /// <param name="searchString">
        /// An optional string parameter that will search for a match in 
        /// LastName, FirstName, PositionName, IdNumber.
        /// Non-alphanumeric characters will be removed prior to comparison. Both the searchString and the comparison text will be converted to lower case prior to comparing.
        /// </param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [HttpGet]
        [Route("Members/Index")]
        public IActionResult Index(string sortOrder, string searchString, int page = 1)
        {
            MemberIndexListViewModel vm = unitOfWork.Members.GetMemberIndexListViewModel();
            vm.CurrentSort = sortOrder;
            vm.MemberLastNameSort = String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
            vm.MemberFirstNameSort = sortOrder == "FirstName" ? "firstName_desc" : "FirstName";
            vm.IdNumberSort = sortOrder == "IDNumber" ? "idNumber_desc" : "IDNumber";
            vm.PositionNameSort = sortOrder == "PositionName" ? "positionName_desc" : "PositionName";
            vm.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                  || char.IsWhiteSpace(c)
                                  || c == '-')));
                string lowerString = new string(arr);
                lowerString = lowerString.ToLower();
                vm.Members = vm.Members
                    .Where(x => x.LastName.ToLower().Contains(lowerString)
                    || x.FirstName.ToLower().Contains(lowerString)
                    || x.PositionName.ToLower().Contains(lowerString)
                    || x.IdNumber.Contains(lowerString));
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
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = searchString == null ? unitOfWork.Members.GetAll().Count() : vm.Members.Count()
            };
            ViewBag.Title = "BlueDeck Members Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            vm.Members = vm.Members.Skip((page - 1) * PageSize).Take(PageSize);
            return View(vm);
        }

        /// <summary>
        /// GET: Members/Details/5.
        /// </summary>
        /// <param name="id">The identifier for a Member.</param>
        /// <param name="returnUrl">An optional return Url</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [HttpGet]
        [Route("Members/Details/{id:int}")]
        public IActionResult Details(int? id, string returnUrl)
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
            ViewBag.ReturnUrl = returnUrl;
            return View(member);
        }

        /// <summary>
        /// GET: Members/Create.
        /// </summary>
        /// <param name="returnUrl">An optional returnUrl parameter.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [HttpGet]
        [Route("Members/Create")]
        public IActionResult Create(string returnUrl)
        {
            MemberAddEditViewModel vm = new MemberAddEditViewModel(new Member(),
                unitOfWork.Positions.GetAllPositionSelectListItems(),
                unitOfWork.MemberRanks.GetMemberRankSelectListItems(),
                unitOfWork.MemberGenders.GetMemberGenderSelectListItems(),
                unitOfWork.MemberRaces.GetMemberRaceSelectListItems(),
                unitOfWork.MemberDutyStatus.GetMemberDutyStatusSelectListItems(),
                unitOfWork.PhoneNumberTypes.GetPhoneNumberTypeSelectListItems(),
                unitOfWork.AppStatuses.GetApplicationStatusSelectListItems());
            ViewBag.Title = "Create New Member";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }

        /// <summary>
        /// POST: Members/Create.
        /// </summary>
        /// <param name="form">A <see cref="MemberAddEditViewModel"/> with certain fields bound on submit</param>
        /// <param name="returnUrl">An optional return URL.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Members/Create")]
        public IActionResult Create([Bind(
            "FirstName," +
            "LastName," +
            "MiddleName," +
            "MemberRank," +
            "DutyStatusId," +
            "MemberRace," +
            "MemberGender," +
            "PositionId," +
            "TempPositionId," +
            "IdNumber," +
            "Email," +
            "LDAPName," +
            "PayrollID," +
            "HireDate," +
            "OrgPositionNumber," +
            "AppStatusId," +
            "ContactNumbers," +
            "IsUser," +
            "IsComponentAdmin," +
            "IsGlobalAdmin")
            ] MemberAddEditViewModel form, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                form.Positions = unitOfWork.Positions.GetAllPositionSelectListItems();
                form.RaceList = unitOfWork.MemberRaces.GetMemberRaceSelectListItems();
                form.RankList = unitOfWork.MemberRanks.GetMemberRankSelectListItems();
                form.GenderList = unitOfWork.MemberGenders.GetMemberGenderSelectListItems();
                form.DutyStatus = unitOfWork.MemberDutyStatus.GetMemberDutyStatusSelectListItems();
                form.PhoneNumberTypes = unitOfWork.PhoneNumberTypes.GetPhoneNumberTypeSelectListItems();
                form.AppStatuses = unitOfWork.AppStatuses.GetApplicationStatusSelectListItems();
                ViewBag.Title = "Create Member - Corrections Required";
                ViewBag.Status = "Warning!";
                ViewBag.Message = "You must correct the fields indicated.";
                return View(form);
            }
            else
            {
                // TODO: Member addition checks? Duplicate Name/Badge Numbers?
                form.CreatedById = Convert.ToInt32(User.Claims.FirstOrDefault(claim => claim.Type == "MemberId").Value);
                form.LastModifiedById = Convert.ToInt32(User.Claims.FirstOrDefault(claim => claim.Type == "MemberId").Value);
                form.CreatedDate = DateTime.Now;
                form.LastModified = DateTime.Now;
                unitOfWork.Members.UpdateMember(form);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Member successfully created.";
                if (returnUrl != "")
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        /// <summary>
        /// Members/Edit/5
        /// </summary>
        /// <param name="id">The identity of the Member being edited.</param>
        /// <param name="returnUrl">An optional return URL</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [HttpGet]
        [Authorize("CanEditUser")]
        [Route("Members/Edit/{id:int}")]
        public IActionResult Edit(int? id, string returnUrl)
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
                unitOfWork.PhoneNumberTypes.GetPhoneNumberTypeSelectListItems(),
                unitOfWork.AppStatuses.GetApplicationStatusSelectListItems());
            ViewBag.Title = "Edit Member";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }

        /// <summary>
        /// POST: Members/Edit/5
        /// </summary>
        /// <param name="id">The MemberId for the <see cref="Member"/> being edited</param>
        /// <param name="form">The <see cref="MemberAddEditViewModel"/> object to which the POSTed form is Bound</param>
        /// <param name="returnUrl">An optional return URL</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Members/Edit/{id:int}")]
        public IActionResult Edit(int id, [Bind(
            "MemberId," +
            "FirstName," +
            "LastName," +
            "MiddleName," +
            "MemberRank," +
            "DutyStatusId," +
            "MemberGender," +
            "MemberRace," +
            "PositionId," +
            "TempPositionId," +
            "IdNumber," +
            "Email," +
            "LDAPName," +
            "PayrollID," +
            "HireDate," +
            "OrgPositionNumber," +
            "AppStatusId," +
            "ContactNumbers," +
            "IsUser," +
            "IsComponentAdmin," +
            "IsGlobalAdmin," +
            "Creator," +
            "CreatedDate," +
            "LastModifiedBy," +
            "LastModified")] MemberAddEditViewModel form, string returnUrl)
        {

            if (!ModelState.IsValid)
            {
                // Model Validation failed, repopulate the ViewModel's List data and return the View
                form.Positions = unitOfWork.Positions.GetAllPositionSelectListItems();
                form.RaceList = unitOfWork.MemberRaces.GetMemberRaceSelectListItems();
                form.RankList = unitOfWork.MemberRanks.GetMemberRankSelectListItems();
                form.GenderList = unitOfWork.MemberGenders.GetMemberGenderSelectListItems();
                form.DutyStatus = unitOfWork.MemberDutyStatus.GetMemberDutyStatusSelectListItems();
                form.AppStatuses = unitOfWork.AppStatuses.GetApplicationStatusSelectListItems();
                form.PhoneNumberTypes = unitOfWork.PhoneNumberTypes.GetPhoneNumberTypeSelectListItems();
                ViewBag.Title = "Edit Member - Corrections Required";
                ViewBag.Status = "Warning!";
                ViewBag.Message = "You must correct the fields indicated.";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            else
            {
                try
                {
                    form.LastModifiedById = Convert.ToInt32(User.Claims.FirstOrDefault(claim => claim.Type == "MemberId").Value);
                    form.LastModified = DateTime.Now;
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
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// GET: Member/Delete/5
        /// </summary>
        /// <param name="id">The MemberId of the <see cref="Member"/> being deleted</param>
        /// <param name="returnUrl">An optional URI to redirect the user via the "Cancel" button or as a pass-through.</param>
        /// <returns>An <see cref="IActionResult"/> that prompts the user to confirm the deletion of the Member with the given id</returns>
        [HttpGet]
        [Authorize("IsGlobalAdmin")]
        [Route("Members/Delete/{id:int}")]
        public IActionResult Delete(int? id, string returnUrl)
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
            ViewBag.ReturnUrl = returnUrl;
            return View(m);
        }

        /// <summary>
        /// POST: Members/Delete/5
        /// </summary>
        /// <param name="id">The MemberId of the <see cref="Member"/> being deleted</param>
        /// <returns>An <see cref="IActionResult"/> that redirects to <see cref="MembersController.Index"/> on successful deletion of a Member.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Members/Delete/{id:int}")]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            unitOfWork.Members.Remove(id);
            unitOfWork.Complete();
            TempData["Status"] = "Success!";
            TempData["Message"] = "Member successfully deleted.";
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
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
