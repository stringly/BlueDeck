using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models;
using BlueDeck.Models.Types;
using System.Collections.Generic;
using BlueDeck.Models.ViewModels;
using System;

namespace BlueDeck.Controllers
{
    /// <summary>
    /// Serves Views and data that renders in <a href="http://www.getorgchart.com/Documentation#separationMixedHierarchyNodes" >GetOrgChart</a> Plugin
    /// </summary>
    /// <seealso cref="Controller" />
    [ApiExplorerSettings(IgnoreApi = true)]
    public class OrgChartController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BlueDeck.Controllers.HomeController"/> class.
        /// </summary>
        /// <param name="unit">An <see cref="T:BlueDeck.Models.IUnitOfWork"/></param>
        public OrgChartController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// Gets the components. (async, JSON result from the GetOrgChart JQuery Library)
        /// </summary>
        /// <param name="parentComponentId">The component id of the Top-level component in the desired result set.</param>
        /// <returns>A JSON object containing a list of <see cref="ChartableComponentWithMember"/> objects.</returns>
        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetComponents(int parentComponentId)
        {
            List<ChartableComponentWithMember> components = unitOfWork.Components.GetOrgChartComponentsWithMembers(parentComponentId);
            List<ComponentSelectListItem> items = unitOfWork.Components.GetComponentSelectListItems();
            
            return Json(components);
        }

        /// <summary>
        /// Gets the component select list items.
        /// </summary>
        /// <remarks>
        /// This method retrieves a list of <see cref="ComponentSelectListItem"/> items for use in the Component select list drop-down.
        /// </remarks>
        /// <returns>A JSON object containing all <see cref="ComponentSelectListItem"/> items.</returns>
        [HttpGet]
        public JsonResult GetComponentSelectListItems()
        {            
            return Json(unitOfWork.Components.GetComponentSelectListItems());
        }
        /// <summary>
        /// GET /OrgChart/Index
        /// </summary>
        /// <remarks>
        /// Retrieves the OrgChart/Index view, which loads the GetOrgChart plugin.
        /// </remarks>
        /// <returns>An <see cref="IActionResult"/></returns>
        [HttpGet]
        [Route("OrgChart/Index/{id:int?}")]
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