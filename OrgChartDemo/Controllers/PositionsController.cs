using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using OrgChartDemo.Models.ViewModels;
using OrgChartDemo.Models.Types;
using OrgChartDemo.Persistence;
using System.Collections.Generic;
using System;

namespace OrgChartDemo.Controllers
{
    /// <summary>
    /// Controller for Position CRUD actions
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    public class PositionsController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Controllers.PositionsController"/> class.
        /// </summary>
        /// <param name="unit"><see cref="T:OrgChartDemo.Persistence.UnitOfWork"/>.</param>
        public PositionsController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// TODO: Add "Members" Nav choice to List Item Options "Edit/Delete/Members" and wire to Members view
        /// <summary>
        /// GET: Positions
        /// </summary>
        /// <remarks>
        /// This View requires an <see cref="T:IEnumerable{T}"/> list of <see cref="T:OrgChartDemo.Models.ViewModels.PositionWithMemberCountItem"/>
        /// </remarks>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Index(string sortOrder, string searchString)
        {
            PositionIndexListViewModel vm = new PositionIndexListViewModel(unitOfWork.Positions.GetPositionsWithMembers().ToList());
            vm.CurrentSort = sortOrder;
            vm.PositionNameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            vm.ParentComponentNameSort = sortOrder == "ParentComponentName" ? "parentName_desc" : "ParentComponentName";
            vm.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                vm.Positions = vm.Positions
                    .Where(x => x.PositionName.Contains(searchString) || x.ParentComponentName.Contains(searchString) || x.JobTitle.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    vm.Positions = vm.Positions.OrderByDescending(x => x.PositionName);
                    break;
                case "ParentComponentName":
                    vm.Positions = vm.Positions.OrderBy(x => x.ParentComponentName);
                    break;
                case "parentName_desc":
                    vm.Positions = vm.Positions.OrderByDescending(x => x.ParentComponentName);
                    break;
                default:
                    vm.Positions = vm.Positions.OrderBy(x => x.PositionName);
                    break;
            }
            return View(vm);
        }

        /// <summary>
        /// GET: Positions/Details/5.
        /// </summary>
        /// <param name="id">The identifier for a Position.</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = unitOfWork.Positions
                .SingleOrDefault(m => m.PositionId == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        /// <summary>
        /// GET: Positions/Create.
        /// </summary>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Create()
        {
            PositionWithComponentListViewModel vm = new PositionWithComponentListViewModel(new Position()) { 
                Components = unitOfWork.Components.GetComponentSelectListItems()
                };            
            return View(vm);
        }

        /// <summary>
        /// POST: Positions/Create.
        /// </summary>
        /// <param name="form">A <see cref="T:OrgChartDemo.Models.ViewModels.PositionWithComponentListViewModel"/> with certain fields bound on submit</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("PositionName,LineupPosition,ParentComponentId,JobTitle,IsManager,IsUnique")] PositionWithComponentListViewModel form)
        {
            int errors = 0;
            Component targetParentComponent = unitOfWork.Components.SingleOrDefault(c => c.ComponentId == form.ParentComponentId);
            Position p = new Position {
                ParentComponent = targetParentComponent,
                Name = form.PositionName,
                IsUnique = form.IsUnique,
                JobTitle = form.JobTitle,
                IsManager = form.IsManager,
                LineupPosition = form.LineupPosition               
                };

            if (!ModelState.IsValid) {
                errors++;
            }
            else if (unitOfWork.Positions.SingleOrDefault(x => x.Name == form.PositionName) != null) {                    
                ViewBag.Message = $"A Position with the name {form.PositionName} already exists. Use a different Name.\n";
                errors++;
            }
            // check if user is attempting to add "Manager" position to the ParentComponent
            else if (form.IsManager) {
                // check if the Parent Component of the position already has a Position designated as "Manager"
                if (unitOfWork.Positions.SingleOrDefault(c => c.ParentComponent.ComponentId == form.ParentComponentId && c.IsManager == true) != null) {                        
                    ViewBag.Message += $"{p.ParentComponent.Name} already has a Position designated as Manager. Only one Manager Position is permitted.\n";
                    errors++;
                }
            }            
            if (errors == 0) {
                targetParentComponent = unitOfWork.Components.SingleOrDefault(c => c.ComponentId == form.ParentComponentId);
                unitOfWork.Positions.UpdatePositionAndSetLineup(p);
                unitOfWork.Complete();
                return RedirectToAction(nameof(Index)); 
            }
            else {
                form.Components = unitOfWork.Components.GetComponentSelectListItems();
                return View(form);
            }
        }

        /// <summary>
        /// Positions/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position position = unitOfWork.Positions.GetPositionWithParentComponent(Convert.ToInt32(id));
            if (position == null)
            {
                return NotFound();
            }
            PositionWithComponentListViewModel vm = new PositionWithComponentListViewModel(position){ 
                Components = unitOfWork.Components.GetComponentSelectListItems()
                };
            return View(vm);
        }

        /// <summary>
        /// POST: Positions/Edit/5
        /// </summary>
        /// <param name="id">The PositionId for the <see cref="T:OrgChartDemo.Models.Position"/> being edited</param>
        /// <param name="form">The <see cref="T:OrgChartDemo.Models.ViewModels.PositionWithComponentListViewModel"/> object to which the POSTed form is Bound</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("PositionId,PositionName,LineupPosition,ParentComponentId,JobTitle,IsManager,IsUnique")] PositionWithComponentListViewModel form)
        {
            int errors = 0;
            Component targetParentComponent = unitOfWork.Components.Find(c => c.ComponentId == form.ParentComponentId).FirstOrDefault();
            Position p = new Position();
            
            if (!ModelState.IsValid) {
                errors++;
            }
            else {            

                if (id != form.PositionId) {
                    return NotFound();
                }
                else if (unitOfWork.Positions.Find(x => x.Name == form.PositionName && x.PositionId != id).FirstOrDefault() != null)
                {
                    // user is attempting to change the name of the position to a name which already exists
                    ViewBag.Message = $"A Position with the name {form.PositionName} already exists. Use a different Name.\n";
                    errors++;
                }
                else if (form.IsManager && unitOfWork.Positions.Find(x => x.ParentComponent.ComponentId == form.ParentComponentId && x.IsManager && x.PositionId != form.PositionId).FirstOrDefault() != null) {
                    // user is attempting to elevate a Position to Manager when the ParentComponent already has a Manager
                    ViewBag.Message += $"{targetParentComponent.Name} already has a Position designated as Manager. You can not elevate this Position.\n";
                    errors++;              
                }
                else if (form.IsUnique == true && p.IsUnique == false && p.Members.Count() > 1) {
                    // user is attempting to make Position unique when multiple members are assigned
                    ViewBag.Message += $"{p.Name} has {p.Members.Count()} current Members. You can't set this Position to Unique with multiple members.\n";
                    errors++;
                }
            }
            // 0 errors should mean all conditions passed
            if (errors == 0) {
                p.PositionId = Convert.ToInt32(form.PositionId);
                p.ParentComponent = targetParentComponent;
                p.Name = form.PositionName;
                p.IsUnique = form.IsUnique;
                p.JobTitle = form.JobTitle;
                p.IsManager = form.IsManager;
                p.LineupPosition = form.LineupPosition;
                unitOfWork.Positions.UpdatePositionAndSetLineup(p);
                unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            } else
            {
                form.Components = unitOfWork.Components.GetComponentSelectListItems();
                return View(form);
            }
            
        }
        
        /// <summary>
        /// GET: Positions/Delete/5
        /// </summary>
        /// <param name="id">The PositionId of the <see cref="T:OrgChartDemo.Models.Position"/> being deleted</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Delete(int? id)
        {
            // TODO: Warn or Prevent User from Deleting a Position with assigned Members? Or auto-reassign members to the General Pool?
            if (id == null)
            {
                return NotFound();
            }

            var position = unitOfWork.Positions
                .SingleOrDefault(m => m.PositionId == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        /// TODO: PositionsController: Handle Deleting a Position with members? Move all to Unassigned?
        /// <summary>
        /// POST: Positions/Delete/5
        /// </summary>
        /// <param name="id">The PositionId of the <see cref="T:OrgChartDemo.Models.Position"/> being deleted</param>
        /// <returns>An <see cref="T:IActionResult"/> that redirects to <see cref="T:OrgChartDemo.Controllers.PositionsController.Index"/> on successful deletion of a Position.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            unitOfWork.Positions.RemovePositionAndReassignMembers(id);
            unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Determines if a Position exists with the provided PositionId .
        /// </summary>
        /// <param name="id">The PositionId of the <see cref="T:OrgChartDemo.Models.Position"/></param>
        /// <returns>True if a <see cref="T:OrgChartDemo.Models.Position"/> with the given id exists</returns>
        private bool PositionExists(int id)
        {
            return (unitOfWork.Positions.Find(e => e.PositionId == id) != null);
        }

        public IActionResult GetPositionLineupViewComponent(int componentId, int positionBeingEditedId = 0)
        {
            List<PositionLineupItem> positions = unitOfWork.Components.GetPositionLineupItemsForComponent(componentId);
            if (positionBeingEditedId == 0){ 
                PositionLineupViewComponentViewModel vm = new PositionLineupViewComponentViewModel(positions);
                return ViewComponent("PositionLineup", vm);
            }
            else
            {
                Position positionToEdit = unitOfWork.Positions.GetPositionWithParentComponent(positionBeingEditedId);
                PositionLineupViewComponentViewModel vm = new PositionLineupViewComponentViewModel(positions, positionToEdit);
                return ViewComponent("PositionLineup", vm);
            }
        }
    }
}
