using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using OrgChartDemo.Models.DocGenerators;
using OrgChartDemo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OrgChartDemo.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork unitOfWork;

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
                if (claimMemberId != 0)
                {
                    ViewBag.Title = "BlueDeck Home";
                    HomePageViewModel vm = unitOfWork.Members.GetHomePageViewModelForMember(claimMemberId);
                    return View(vm);
                }
            }
            return RedirectToAction(nameof(About));
        }
        [AllowAnonymous]
        public IActionResult About()
        {
            ViewBag.Title = "About BlueDeck";
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
        public IActionResult DownloadAlphaRoster()
        {
            AlphaRosterGenerator gen = new AlphaRosterGenerator();
            return File(gen.Generate(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Alpha Roster Test.docx");
        }
    }
}