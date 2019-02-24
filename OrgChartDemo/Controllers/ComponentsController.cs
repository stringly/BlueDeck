using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Controllers
{
    /// <summary>
    /// Controller for Component CRUD actions
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    public class ComponentsController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Controllers.ComponentsController"/> class.
        /// </summary>
        /// <param name="unit"><see cref="T:OrgChartDemo.Persistence.UnitOfWork"/>.</param>
        public ComponentsController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// GET: Components
        /// </summary>
        /// <remarks>
        /// This View requires an <see cref="T:IEnumerable{T}"/> list of <see cref="T:OrgChartDemo.Models.ViewModels.ComponentIndexListViewModel"/>
        /// </remarks>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="searchString">The search string.</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Index(string sortOrder, string searchString)
        {
            ComponentIndexListViewModel vm = new ComponentIndexListViewModel(unitOfWork.Components.GetComponentsWithChildren().ToList());
            vm.CurrentSort = sortOrder;
            vm.ComponentNameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            vm.ParentComponentNameSort = sortOrder == "ParentComponentName" ? "parentName_desc" : "ParentComponentName";
            vm.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                vm.Components = vm.Components
                    .Where(x => x.ComponentName.Contains(searchString) || x.ParentComponentName.Contains(searchString) || x.Acronym.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    vm.Components = vm.Components.OrderByDescending(x => x.ComponentName);
                    break;
                case "ParentComponentName":
                    vm.Components = vm.Components.OrderBy(x => x.ParentComponentName);
                    break;
                case "parentName_desc":
                    vm.Components = vm.Components.OrderByDescending(x => x.ParentComponentName);
                    break;
                default:
                    vm.Components = vm.Components.OrderBy(x => x.ComponentName);
                    break;
            }

            return View(vm);
        }

        /// <summary>
        /// GET: Components/Details/5.
        /// </summary>
        /// <param name="id">The identifier for a Component.</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var component = unitOfWork.Components.SingleOrDefault(m => m.ComponentId == id);
            if (component == null)
            {
                return NotFound();
            }

            return View(component);
        }
        
        /// <summary>
        /// GET: Component/Create.
        /// </summary>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Create()
        {
            ComponentWithComponentListViewModel vm = new ComponentWithComponentListViewModel(new Component(), unitOfWork.Components.GetAll().ToList());
            return View(vm);
        }

        /// <summary>
        /// POST: Components/Create.
        /// </summary>
        /// <param name="form">A <see cref="T:OrgChartDemo.Models.ViewModels.ComponentWithComponentListViewModel"/> with certain fields bound on submit</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ParentComponentId,ComponentName,LineupPosition,Acronym")] ComponentWithComponentListViewModel form)
        {        
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                Component c = new Component
                {
                    Name = form.ComponentName,
                    Acronym = form.Acronym,
                    ParentComponent = unitOfWork.Components.SingleOrDefault(x => x.ComponentId == form.ParentComponentId),
                };
                // check if a Component with the Name provided already exists and reject if so
                if (unitOfWork.Components.SingleOrDefault(x => x.Name == form.ComponentName) != null)
                {
                    ViewBag.Messaage = $"A Component with the name {form.ComponentName} already exists. Use a different Name.";
                    ComponentWithComponentListViewModel vm = new ComponentWithComponentListViewModel(c, unitOfWork.Components.GetAll().ToList());
                }
                // TODO: rewire to repo method to control setting lineup
                unitOfWork.Components.Add(c);
                unitOfWork.Complete();
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Components/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Component component = unitOfWork.Components.SingleOrDefault(x => x.ComponentId == id);
            if (component == null)
            {
                return NotFound();
            }
            ComponentWithComponentListViewModel vm = new ComponentWithComponentListViewModel(component, unitOfWork.Components.GetAll().ToList());
            return View(vm);
        }

        /// <summary>
        /// POST: Components/Edit/5
        /// </summary>
        /// <param name="id">The ComponentId for the <see cref="T:OrgChartDemo.Models.Component"/> being edited</param>
        /// <param name="form">The <see cref="T:OrgChartDemo.Models.ViewModels.ComponentWithComponentListViewModel"/> object to which the POSTed form is Bound</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ParentComponentId,ComponentName,LineupPosition,Acronym")] ComponentWithComponentListViewModel form )
        {
            Component c = unitOfWork.Components.SingleOrDefault(x => x.ComponentId == id);
            Component targetParentComponent = unitOfWork.Components.SingleOrDefault(x => x.ComponentId == form.ParentComponentId);
            if (id != form.ComponentId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (unitOfWork.Components.SingleOrDefault(x => x.Name == form.ComponentName) != null)
            {
                ViewBag.Messaage = $"A Component with the name {form.ComponentName} already exists. Use a different Name.";
                ComponentWithComponentListViewModel vm = new ComponentWithComponentListViewModel(c, unitOfWork.Components.GetAll().ToList());
                return View(vm);
            }
            else { 
                try
                {
                    // TODO: rewire to Repo method to set sibling lineup
                    c.Name = form.ComponentName;
                    c.ParentComponent = targetParentComponent;
                    c.Acronym = form.Acronym;
                    unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComponentExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// GET: Components/Delete/5
        /// </summary>
        /// <param name="id">The ComponentId of the <see cref="T:OrgChartDemo.Models.Component"/> being deleted</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }            
            Component component = unitOfWork.Components.GetComponentWithChildren(Convert.ToInt32(id));
            
            if (component == null)
            {
                return NotFound();
            }
            else if (component.Positions.Count() > 0)
            {
                int totalMembers = 0;
                foreach (Position p in component.Positions)
                {
                    totalMembers += p.Members.Count();
                }
                ViewBag.Message = $"WARNING: This Component includes {component.Positions.Count()} Positions with a total of {totalMembers} Members.\n"
                                        + "Deleting this Component will also delete all of it's assigned Positions and reassign all Members to 'Unassigned.'";
            }

            return View(component);
        }

        /// <summary>
        /// POST: Components/Delete/5
        /// </summary>
        /// <param name="id">The ComponentId of the <see cref="T:OrgChartDemo.Models.Component"/> being deleted</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Component c = unitOfWork.Components.Get(id);
            foreach (Position p in c.Positions)
            {
                if (p.Members.Count() > 0)
                {
                    unitOfWork.Positions.RemovePositionAndReassignMembers(p.PositionId);
                }                         
            }
            unitOfWork.Components.Remove(c);
            unitOfWork.Complete();            
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Determines if a Component exists with the provided ComponentId .
        /// </summary>
        /// <param name="id">The ComponentId of the <see cref="T:OrgChartDemo.Models.Component"/></param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        private bool ComponentExists(int id)
        {
            return (unitOfWork.Components.Find(e => e.ComponentId == id) != null);
        }

        public IActionResult GetComponentLineupViewComponent(int parentComponentId, int componentBeingEditedId = 0)
        {
            // the Ajax request will sent the ComponentId of the desired Parent Component when a component is being created
            List<ComponentPositionLineupItem> components = unitOfWork.Components.GetComponentLineupItemsForComponent(parentComponentId);
            
            if (componentBeingEditedId == 0)
            {                
                ComponentLineupViewComponentViewModel vm = new ComponentLineupViewComponentViewModel(components);
                return ViewComponent("ComponentLineup", vm);
            }
            else
            {
                Component componentToEdit = unitOfWork.Components.Get(componentBeingEditedId);
                ComponentLineupViewComponentViewModel vm = new ComponentLineupViewComponentViewModel(components, componentToEdit);
                return ViewComponent("ComponentLineup", vm);
            }
        }
    }
}
