using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BlueDeck.Controllers
{
    /// <summary>
    /// Controller that handles views for custom error pages.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        /// <summary>
        /// Errors the specified status code.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Returns the view for Server errors handled by the global error handling middleware.
        /// </summary>
        /// <returns></returns>
        [Route("Error/500")]
        public IActionResult Error500()
        {
            return View();
        }
    }
}