using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Types;
using System.Collections.Generic;

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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public JsonResult GetComponents(int parentComponentId)
        {
            List<ChartableComponentWithMember> components = unitOfWork.Components.GetOrgChartComponentsWithMembers(parentComponentId);
            List<ComponentSelectListItem> items = unitOfWork.Components.GetComponentSelectListItems();
            
            return Json(components);
        }

        [HttpGet]
        public JsonResult GetComponentSelectListItems()
        {            
            return Json(unitOfWork.Components.GetComponentSelectListItems());
        }
        /// <summary>
        /// GET /OrgChart/
        /// </summary>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Index()
        {
            List<ComponentSelectListItem> componentList = unitOfWork.Components.GetComponentSelectListItems();
            ViewBag.Title = "Organization Charts";
            return View(componentList);
        }
        
    }
}