using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models;
using BlueDeck.Models.DocGenerators;
using BlueDeck.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BlueDeck.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public HomeController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }
        public IActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            if (identity.HasClaim(claim => claim.Type == "MemberId"))
            {                
                var claimMemberId = Convert.ToInt32(identity.Claims.FirstOrDefault(claim => claim.Type == "MemberId").Value.ToString());
                // if a user has registered for an account, but the admin has not activated it yet, the User will have a MemberId, but
                // will not have the "User" Role Claim until their account is activated.
                if (claimMemberId != 0 && User.IsInRole("User"))
                    {
                        ViewBag.Title = "BlueDeck Home";
                        HomePageViewModel vm = unitOfWork.Members.GetHomePageViewModelForMember(claimMemberId);                        
                        return View(vm);
                    }
                else if (claimMemberId != 0)
                {
                    // Users with BlueDeck accounts Pending activation should be redirected to the "Pending" View
                    // If the User already has an account (existed at development), then their account status should be set to '1' (New)
                    // If they are accessing the app for the first time, the status will be set to "Pending" so it shows in the admin panel for activation.
                    Member currentMember = unitOfWork.Members.Get(claimMemberId);
                    int newStatusId = unitOfWork.AppStatuses.Find(x => x.StatusName == "New").FirstOrDefault()?.AppStatusId ?? 0;
                    int pendingStatusId = unitOfWork.AppStatuses.Find(x => x.StatusName == "Pending").FirstOrDefault()?.AppStatusId ?? 0;
                    if(currentMember.AppStatusId == newStatusId)
                    {
                        currentMember.AppStatusId = pendingStatusId;
                        currentMember.LastModified = DateTime.Now;
                        currentMember.LastModifiedById = claimMemberId;                        
                        unitOfWork.Complete();
                    }
                    return RedirectToAction(nameof(Pending));
                }
                
            }
            return RedirectToAction(nameof(About));
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("Home/About")]
        public IActionResult About()
        {
            ViewBag.Title = "About BlueDeck";
            return View();
        }
        // TODO: Auth handler for User Role?
        [HttpGet]
        [Route("Home/Pending")]
        public IActionResult Pending()
        {
            if (User.IsInRole("User")) // in case a user navigates manually to Pending
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Title = "Registration Pending";
            return View();
        }
        
        public IActionResult GetMemberSearchViewComponent(string searchString)
        {
            char[] arr = searchString.ToCharArray();

            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) 
                                  || char.IsWhiteSpace(c) 
                                  || c == '-')));
            searchString = new string(arr);
            
            if (!string.IsNullOrEmpty(searchString))
            {
                List<Member> initial = unitOfWork.Members.GetMembersWithPositions().ToList();
                initial = initial.Where(
                    x => x.LastName.ToLower().Contains(searchString.ToLower())
                    || x.FirstName.ToLower().Contains(searchString.ToLower())
                    || x.IdNumber.ToLower().Contains(searchString.ToLower())
                    || x.Position.Name.ToLower().Contains(searchString.ToLower()))
                    .ToList();
                HomePageMemberSearchResultViewComponentViewModel vm = new HomePageMemberSearchResultViewComponentViewModel(initial);
                return ViewComponent("HomePageMemberSearchResult", vm);
            }
            else
            {
                HomePageMemberSearchResultViewComponentViewModel vm = new HomePageMemberSearchResultViewComponentViewModel(new List<Member>());
                return ViewComponent("HomePageMemberSearchResult", vm);
            }
        }

        public IActionResult GetDemograpicSearchResultViewComponent(int SelectedDemographicComponent, List<int> SelectedRanks, int SelectedGender, List<int> SelectedRaces)
        {
            Component c = unitOfWork.Components.GetComponentForDemographics(SelectedDemographicComponent);
            if (c != null)
            {
                ComponentDemographicTableViewComponentViewModel vm =
                    new ComponentDemographicTableViewComponentViewModel(
                        c,
                        unitOfWork.MemberGenders.GetMemberGenderSelectListItems().ToList().Where(x => (SelectedGender == 0 || x.MemberGenderId == SelectedGender)).ToList(),
                        unitOfWork.MemberRaces.GetMemberRaceSelectListItems().ToList().Where(x => (SelectedRaces.Count() == 0 || SelectedRaces.Contains(x.MemberRaceId))).ToList(),
                        unitOfWork.MemberRanks.GetMemberRankSelectListItems().ToList().Where(x => (SelectedRanks.Count() == 0 || SelectedRanks.Contains(x.MemberRankId))).ToList());
                return ViewComponent("ComponentDemographicTable", vm);
            }
            else
            {
                return null;
            }

        }
        [HttpGet]
        [Route("Home/Register")]
        public IActionResult Register()
        {
            var identity = User.Identities.FirstOrDefault(x => x.IsAuthenticated);
            string logonName = identity.Name.Split('\\')[1];
            int pendingStatusId = unitOfWork.AppStatuses.Find(x => x.StatusName == "Pending").FirstOrDefault()?.AppStatusId ?? 0;
            Member newMember = new Member()
            {
                Email = $"{logonName}@co.pg.md.us",
                LDAPName = logonName,
                AppStatusId = pendingStatusId
            };
            MemberAddEditViewModel vm = new MemberAddEditViewModel(newMember,
                unitOfWork.Positions.GetAllPositionSelectListItems(),
                unitOfWork.MemberRanks.GetMemberRankSelectListItems(),
                unitOfWork.MemberGenders.GetMemberGenderSelectListItems(),
                unitOfWork.MemberRaces.GetMemberRaceSelectListItems(),
                unitOfWork.MemberDutyStatus.GetMemberDutyStatusSelectListItems(),
                unitOfWork.PhoneNumberTypes.GetPhoneNumberTypeSelectListItems(),
                unitOfWork.AppStatuses.GetApplicationStatusSelectListItems());
            ViewBag.Title = "Register";
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("FirstName,LastName,MiddleName,MemberRank,DutyStatusId,MemberRace,MemberGender,PositionId,IdNumber,Email,LDAPName,AppStatusId,ContactNumbers")] MemberAddEditViewModel form)
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
                var identity = (ClaimsIdentity)User.Identity;
                form.LastModified = DateTime.Now;
                form.CreatedDate = DateTime.Now;
                form.CreatedById = 1; // hack... just set this to me for new registrations
                form.LastModifiedById = 1;
                // TODO: Member addition checks? Duplicate Name/Badge Numbers?
                unitOfWork.Members.UpdateMember(form);
                unitOfWork.Complete();
                return RedirectToAction(nameof(Pending));
            }            
        }
        [HttpGet]
        [Route("Home/DownloadAlphaRoster/{id:int}")]
        public IActionResult DownloadAlphaRoster(int id)
        {
            AlphaRosterGenerator gen = new AlphaRosterGenerator();
            gen.Members = unitOfWork.Components.GetMembersRosterForComponentId(id);
            gen.ComponentName = unitOfWork.Components.Get(id).Name;
            string fileName = $"{unitOfWork.Components.Get(id).Name} Alpha Roster {DateTime.Now.ToString("MM'-'dd'-'yy")}.docx";

            return File(gen.Generate(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
        }
        [HttpGet]
        [Route("Home/DownloadComponentRoster/{id:int}")]
        public IActionResult DownloadComponentRoster(int id)
        {
            TraditionalRosterGenerator gen = new TraditionalRosterGenerator(unitOfWork.Components.GetComponentsAndChildrenWithParentSP(id));
            //ComponentRosterGenerator gen = new ComponentRosterGenerator(unitOfWork.Components.GetComponentsAndChildrenWithParentSP(id));
            string fileName = $"{unitOfWork.Components.Get(id).Name} Roster {DateTime.Now.ToString("MM'-'dd'-'yy")}.docx";
            return File(gen.Generate(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
        }
        [HttpGet]
        [Route("Home/DownloadOrganizationChart/{id:int}")]
        public IActionResult DownloadOrganizationChart(int id)
        {
            OrgChartGenerator gen = new OrgChartGenerator(unitOfWork.Components.GetOrgChartComponentsWithMembersNoMarkup(id));
            string fileName = $"{unitOfWork.Components.Get(id).Name} Organization Chart {DateTime.Now.ToString("MM'-'dd'-'yy")}.docx";
            return File(gen.Generate(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
        }
    }
}