
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrgChartDemo.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                switch (statusCode.Value)
                {
                    case 403:
                        ViewBag.Title = "Not Authorized";
                        return View("NotAuthorized");
                    case 404:
                        ViewBag.Title = "Not Found";
                        return View("NotFound");
                    
                }
            }
            return View();
        }
    }
}