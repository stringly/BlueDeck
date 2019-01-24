using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using OrgChartDemo.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace OrgChartDemo.Controllers
{
    public class RosterController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Controllers.RosterController"/> class.
        /// </summary>
        /// <param name="unit">An <see cref="T:OrgChartDemo.Models.IUnitOfWork"/></param>
        public RosterController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }
        public IActionResult Index()
        {
            var vm = new RosterManagerViewModel();
            vm.Components = unitOfWork.Components.GetComponentSelectListItems();
            return View(vm);
        }

        /// <summary>
        /// Gets the components. (async, JSON result from the GetOrgChart JQuery Library)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetComponents(int componentId)
        {
            // TODO: update the repo method to retrieve data reequired for this view            
            List<Component> result = unitOfWork.Components.GetComponentAndChildren(componentId, new List<Component>());
            return Json(result);
        }

        public IActionResult GetRosterViewComponent(int componentId){
            List<Component> result = unitOfWork.Components.GetComponentAndChildren(componentId, new List<Component>());            
            return ViewComponent("RosterManager", result.OrderBy(x => x.ComponentId).ToList());    
        }
    }
}