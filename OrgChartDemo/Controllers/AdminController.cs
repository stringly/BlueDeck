using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.Controllers
{
    [Authorize("IsGlobalAdmin")]
    public class AdminController : Controller
    {
        private IUnitOfWork unitOfWork;
        public int PageSize = 25;

        public AdminController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        public IActionResult Index(string returnUrl)
        {
            AdminIndexViewModel vm = new AdminIndexViewModel();
            vm.GlobalAdmins = unitOfWork.Members.GetGlobalAdmins();
            vm.PendingAccounts = unitOfWork.Members.GetPendingAccounts();
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Title = "BlueDeck Admin - Main Page";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            return View(vm);
        }

        public IActionResult MemberIndex(string sortOrder, string searchString, int page = 1)
        {
            AdminMemberIndexListViewModel vm = unitOfWork.Members.GetAdminMemberIndexListViewModel();
            vm.CurrentSort = sortOrder;
            vm.MemberLastNameSort = String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
            vm.MemberFirstNameSort = sortOrder == "FirstName" ? "firstName_desc" : "FirstName";
            vm.IdNumberSort = sortOrder == "IDNumber" ? "idNumber_desc" : "IDNumber";
            vm.PositionNameSort = sortOrder == "PositionName" ? "positionName_desc" : "PositionName";
            vm.IsUserRoleFilter = sortOrder == "IsUserRoleOnly" ? "IsUserRoleAny" : "IsUserRoleOnly";
            vm.IsComponentAdminRoleFilter = sortOrder == "IsComponentAdminOnly" ? "IsComponentAdminAny" : "IsComponentAdminOnly";
            vm.IsGlobalAdminRoleFilter = sortOrder == "IsGlobalAdminOnly" ? "IsGlobalAdminAny" : "IsGlobalAdminOnly";
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
                case "IsUserRoleOnly":
                    vm.Members = vm.Members.Where(x => x.IsUser);
                    break;
                case "IsComponentAdminOnly":
                    vm.Members = vm.Members.Where(x => x.IsComponentAdmin);
                    break;
                case "IsGlobalAdminOnly":
                    vm.Members = vm.Members.Where(x => x.IsGlobalAdmin);
                    break;
                default:
                    vm.Members = vm.Members.OrderBy(x => x.AccountStateId).ThenBy(x => x.LastName);
                    break;
            }
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = searchString == null ? unitOfWork.Members.GetAll().Count() : vm.Members.Count()
            };
            ViewBag.Title = "BlueDeck Admin - Members Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            vm.Members = vm.Members.Skip((page - 1) * PageSize).Take(PageSize);

            return View(vm);            
        }

        public IActionResult PositionIndex(string sortOrder, string searchString, int page = 1)
        {
            AdminPositionIndexListViewModel vm = unitOfWork.Positions.GetAdminPositionIndexListViewModel();
            vm.CurrentSort = sortOrder;
            vm.PositionNameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            vm.ParentComponentNameSort = sortOrder == "ParentComponentName" ? "parentName_desc" : "ParentComponentName";
            vm.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) 
                                  || char.IsWhiteSpace(c) 
                                  || c == '-')));
                string lowerString = new string(arr);
                lowerString = lowerString.ToLower();
                vm.Positions = vm.Positions
                    .Where(x => x.PositionName.ToLower().Contains(lowerString) 
                    || x.ParentComponentName.ToLower().Contains(lowerString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    vm.Positions = vm.Positions.OrderByDescending(x => x.PositionName);
                    break;
                case "parentName_desc":
                    vm.Positions = vm.Positions.OrderByDescending(x => x.ParentComponentName);
                    break;
                case "ParentComponentName":
                    vm.Positions = vm.Positions.OrderBy(x => x.ParentComponentName).ThenBy(x => x.LineupPosition);
                    break;
            }
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = searchString == null ? unitOfWork.Positions.GetAll().Count() : vm.Positions.Count()
            };
            ViewBag.Title = "BlueDeck Admin - Positions Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            vm.Positions = vm.Positions.Skip((page - 1) * PageSize).Take(PageSize);

            return View(vm);  
        }

        public IActionResult ComponentIndex(string sortOrder, string searchString, int page = 1)
        {
            AdminComponentIndexListViewModel vm = unitOfWork.Components.GetAdminComponentIndexListViewModel();
            vm.CurrentSort = sortOrder;
            vm.NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            vm.ParentComponentNameSort = sortOrder == "ParentComponentName" ? "parentName_desc" : "ParentComponentName";
            vm.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) 
                                  || char.IsWhiteSpace(c) 
                                  || c == '-')));
                string lowerString = new string(arr);
                lowerString = lowerString.ToLower();
                vm.Components = vm.Components
                    .Where(x => x.ComponentName.ToLower().Contains(lowerString) 
                    || x.ParentComponentName.ToLower().Contains(lowerString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    vm.Components = vm.Components.OrderByDescending(x => x.ComponentName);
                    break;
                case "parentName_desc":
                    vm.Components = vm.Components.OrderByDescending(x => x.ParentComponentName);
                    break;
                case "ParentComponentName":
                    vm.Components = vm.Components.OrderBy(x => x.ParentComponentName).ThenBy(x => x.LineupPosition);
                    break;
            }
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = searchString == null ? unitOfWork.Components.GetAll().Count() : vm.Components.Count()
            };
            ViewBag.Title = "BlueDeck Admin - Components Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            vm.Components = vm.Components.Skip((page - 1) * PageSize).Take(PageSize);

            return View(vm); 
        }

        public IActionResult AppStatusIndex()
        { 
            ViewBag.Title = "BlueDeck Admin - Application Status Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            ViewBag.Enumeration = true;
            return View(unitOfWork.AppStatuses.GetAppStatusesWithMemberCount());
        }

        public IActionResult DutyStatusIndex()
        {
            ViewBag.Title = "BlueDeck Admin - Duty Status Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            ViewBag.Enumeration = true;
            return View(unitOfWork.MemberDutyStatus.GetDutyStatusesWithMemberCount());
        }

        public IActionResult GenderIndex()
        {
            ViewBag.Title = "BlueDeck Admin - Gender Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            ViewBag.Enumeration = true;
            return View(unitOfWork.MemberGenders.GetGendersWithMembers());
        }

        public IActionResult RaceIndex()
        {
            ViewBag.Title = "BlueDeck Admin - Race Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            ViewBag.Enumeration = true;
            return View(unitOfWork.MemberRaces.GetRacesWithMembers());
        }

        public IActionResult RankIndex()
        {
            ViewBag.Title = "BlueDeck Admin - Rank Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            ViewBag.Enumeration = true;
            return View(unitOfWork.MemberRanks.GetRanksWithMembers());
        }

        public IActionResult PhoneTypeIndex()
        {
            ViewBag.Title = "BlueDeck Admin - Phone Types Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            ViewBag.Enumeration = true;
            return View(unitOfWork.PhoneNumberTypes.GetPhoneNumberTypesWithPhoneNumbers());
        }

        public IActionResult RegisterUser(int id, string returnUrl)
        {
            Member m = unitOfWork.Members.Get(id);
            if (m != null)
            {
                var identity = (ClaimsIdentity)User.Identity;
                var claimMemberId = Convert.ToInt32(identity.Claims.FirstOrDefault(claim => claim.Type == "MemberId").Value.ToString());
                m.AppStatusId = 3;
                m.LastModified = DateTime.Now;
                m.LastModifiedById = claimMemberId;
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Member account successfully activated.";
            }
            else
            {
                TempData["Status"] = "Warning!";
                TempData["Message"] = "Member account could not be found.";
            }
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult DenyUser(int id, string returnUrl)
        {
            Member m = unitOfWork.Members.Get(id);
            if (m != null)
            {
                var identity = (ClaimsIdentity)User.Identity;
                var claimMemberId = Convert.ToInt32(identity.Claims.FirstOrDefault(claim => claim.Type == "MemberId").Value.ToString());
                m.AppStatusId = 1;
                m.LastModified = DateTime.Now;
                m.LastModifiedById = claimMemberId;
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Member account returned to 'New' status.";
            }
            else
            {
                TempData["Status"] = "Warning!";
                TempData["Message"] = "Member account could not be found.";
            }
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}