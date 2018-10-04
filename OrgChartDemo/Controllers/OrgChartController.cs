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
    /// Serves Views and data that renders in <a href="http://www.getorgchart.com/Documentation#separationMixedHierarchyNodes" >GetOrgChart</a> Plugin
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    public class OrgChartController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Controllers.HomeController"/> class.
        /// </summary>
        /// <param name="unit">An <see cref="T:OrgChartDemo.Models.IUnitOfWork"/></param>
        public OrgChartController(IUnitOfWork unit)
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
            return Json(unitOfWork.Components.GetOrgChartComponentsWithMembers());
        }

        /// <summary>
        /// GET /OrgChart/
        /// </summary>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Index() => View();       
        
    }
}