using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;

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
            return View();
        }
    }
}