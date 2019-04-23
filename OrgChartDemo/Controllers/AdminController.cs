using System;
using System.Linq;
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
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Title = "BlueDeck Admin - Main Page";
            return View();
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
    }
}