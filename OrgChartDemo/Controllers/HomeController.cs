using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using Newtonsoft.Json;

namespace OrgChartDemo.Controllers
{
    /// <summary>
    /// Default Controller, generates orgchart landing page. (Needs to be reassigned)
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class HomeController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Controllers.HomeController"/> class.
        /// </summary>
        /// <param name="unit">An <see cref="T:OrgChartDemo.Models.IUnitOfWork"/></param>
        public HomeController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// Gets the components. (async, JSON result from the GetOrgChart JQuery Library
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetComponents() 
        {            
            return Json(unitOfWork.Components.GetOrgChartComponentsWithoutMembers());
        }

        /// <summary>
        /// /localhost/ root matches here
        /// </summary>
        /// <returns></returns>
        public IActionResult Index() => View(); 
    }
}