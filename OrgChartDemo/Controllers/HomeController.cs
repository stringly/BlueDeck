using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using OrgChartDemo.Models.ViewModels;
using System;
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
                    HomePageViewModel vm = unitOfWork.Members.GetHomePageViewModelForMember(claimMemberId);
                    return View(vm);
                }
            }
            return View();
        }
    }
}