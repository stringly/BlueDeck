using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using OrgChartDemo.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;

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

        public JsonResult ReassignMember(int memberId, int positionId)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (memberId != 0 && positionId != 0)
            {
                Member m = unitOfWork.Members.GetMemberWithPosition(memberId);
                Position oldPosition = unitOfWork.Positions.GetPositionWithParentComponent(m.Position.PositionId);
                Position newPosition = unitOfWork.Positions.GetPositionWithParentComponent(positionId);
                if (m != null && newPosition != null)
                {
                    m.Position = newPosition;
                    unitOfWork.Complete();
                }
                if (newPosition.ParentComponent.ComponentId != oldPosition.ParentComponent.ComponentId)
                {                    
                    RosterManagerViewModelComponent newComponent = new RosterManagerViewModelComponent(unitOfWork.Components.GetComponentWithChildren(newPosition.ParentComponent.ComponentId));
                    RosterManagerViewModelComponent oldComponent = new RosterManagerViewModelComponent(unitOfWork.Components.GetComponentWithChildren(oldPosition.ParentComponent.ComponentId));
                    result.Add("#demographicsgroup_" + newComponent.ComponentId, newComponent.GetDemographicTableForComponentAndChildren().ToString());
                    result.Add("#demographicsgroup_" + oldComponent.ComponentId, oldComponent.GetDemographicTableForComponentAndChildren().ToString());
                    
                }
            }

            return Json(result);
        }
    }
}