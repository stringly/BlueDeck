using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Types;
using System.Collections.Generic;
using OrgChartDemo.Models.ViewModels;
using System;

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
        [AllowAnonymous]
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
        public IActionResult Index(int? componentid)
        {
            OrgChartIndexViewModel vm = new OrgChartIndexViewModel();
            vm.ComponentList = unitOfWork.Components.GetComponentSelectListItems();
            ViewBag.Title = "Organization Charts";
            if (componentid != null)
            {
                vm.SelectedComponentId = Convert.ToInt32(componentid);
            }
            return View(vm);
        }
        
    }
}