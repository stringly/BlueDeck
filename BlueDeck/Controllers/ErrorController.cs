
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BlueDeck.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public IActionResult Error(int? statusCode = null)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            ViewData["ErrorUrl"] = feature?.OriginalPath;
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