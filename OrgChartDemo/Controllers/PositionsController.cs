using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.Controllers
{
    /// <summary>
    /// Controller for Position CRUD actions
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class PositionsController : Controller
    {
        private IComponentRepository repository;
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionsController"/> class.
        /// </summary>
        /// <param name="repo">An <see cref="IComponentRepository"/>.</param>
        public PositionsController(IComponentRepository repo)
        {
            repository = repo;
        }

        /// <summary>
        /// GET: Positions
        /// </summary>
        /// <remarks>
        /// This View requires an <see cref="IEnumerable{T}"/> list of <see cref="PositionWithMemberCountItem"/>
        /// </remarks>
        /// <returns></returns>
        public IActionResult Index()
        {            
            return View(repository.GetPositionListWithMemberCount());
        }

        /// <summary>
        /// GET: Positions/Details/5.
        /// </summary>
        /// <param name="id">The identifier for a Position.</param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = repository.Positions
                .FirstOrDefault(m => m.PositionId == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        /// <summary>
        /// GET: Positions/Create.
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            PositionWithComponentListViewModel vm = new PositionWithComponentListViewModel(new Position(), repository.Components.ToList());
            return View(vm);
        }

        /// <summary>
        /// POST: Positions/Create.
        /// </summary>
        /// <param name="form">A <see cref="PositionWithComponentListViewModel"/> with certain fields bound on submit</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PositionName,ParentComponentId,JobTitle,IsManager,IsUnique")] PositionWithComponentListViewModel form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                Position p = new Position
                {
                    ParentComponent = repository.Components.Where(c => c.ComponentId == form.ParentComponentId).FirstOrDefault(),
                    Name = form.PositionName,
                    IsUnique = form.IsUnique,
                    JobTitle = form.JobTitle,
                    IsManager = form.IsManager
                };
            
                // check if a position with the Name provided already exists and reject if so
                if (repository.Positions.Any(x => x.Name == form.PositionName))
                {
                    PositionWithComponentListViewModel vm = new PositionWithComponentListViewModel(new Position(), repository.Components.ToList());
                    ViewBag.Message = $"A Position with the name {form.PositionName} already exists. Use a different Name.";
                    return View(vm);
                }
                // check if user is attempting to add "Manager" position to the ParentComponent
                else if (form.IsManager)
                {
                    // check if the Parent Component of the position already has a Position designated as "Manager"
                    if (repository.Positions.Any(c => c.ParentComponent.ComponentId == form.ParentComponentId && c.IsManager == true)) {
                        PositionWithComponentListViewModel vm = new PositionWithComponentListViewModel(new Position(), repository.Components.ToList());
                        ViewBag.Message = $"{p.ParentComponent.Name} already has a Position designated as Manager. Only one Manager Position is permitted.";
                        return View(vm);
                    }
                }            
                repository.AddPosition(p);            
                return RedirectToAction(nameof(Index)); 
            }

        }

        /// <summary>
        /// Positions/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position position = repository.Positions.Where(x => x.PositionId == id).FirstOrDefault();
            if (position == null)
            {
                return NotFound();
            }
            PositionWithComponentListViewModel vm = new PositionWithComponentListViewModel(position, repository.Components.ToList());
            return View(vm);
        }

        /// <summary>
        /// POST: Positions/Edit/5
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PositionId,PositionName,ParentComponentId,JobTitle,IsManager,IsUnique")] PositionWithComponentListViewModel form)
        {
            Position p = repository.Positions.Where(x => x.PositionId == id).FirstOrDefault();
            Component targetParentComponent = repository.Components.Find(x => x.ComponentId == form.ParentComponentId);
            if (id != form.PositionId) {
                return NotFound();
            }
            else if (form.IsManager && repository.Positions.Any(x => x.ParentComponent.ComponentId == form.ParentComponentId && x.IsManager && x.PositionId != form.PositionId)) {
                // user is attempting to elevate a Position to Manager when the ParentComponent already has a Manager
                
                ViewBag.Message = $"{targetParentComponent.Name} already has a Position designated as Manager. You can not elevate this Position.";
                PositionWithComponentListViewModel vm = new PositionWithComponentListViewModel(p, repository.Components.ToList());
                return View(vm);                
            }
            else if (form.IsUnique == true && p.IsUnique == false && p.Members.Count() > 1) {
                // user is attempting to make Position unique when multiple members are assigned
                ViewBag.Message = $"{p.Name} has {p.Members.Count()} current Members. You can't set this Position to Unique with multiple members.";
                PositionWithComponentListViewModel vm = new PositionWithComponentListViewModel(p, repository.Components.ToList());
                return View(vm);
            }
            else {
                p.ParentComponent = repository.Components.Where(c => c.ComponentId == form.ParentComponentId).FirstOrDefault();
                p.Name = form.PositionName;
                p.IsUnique = form.IsUnique;
                p.JobTitle = form.JobTitle;
                p.IsManager = form.IsManager;
                repository.EditPosition(p);
            }
            return RedirectToAction(nameof(Index));
        }
        
        /// <summary>
        /// GET: Positions/Delete/5
        /// </summary>
        /// <param name="id">The PositionId of the <see cref="Position"/> being deleted</param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = repository.Positions
                .FirstOrDefault(m => m.PositionId == id);
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
        /// <param name="id">The PositionId of the <see cref="Position"/> being deleted</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {            
            repository.RemovePosition(id);            
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Determines if a Position exists with the provided PositionId .
        /// </summary>
        /// <param name="id">The PositionId of the <see cref="Position"/></param>
        /// <returns></returns>
        private bool PositionExists(int id)
        {
            return repository.Positions.Any(e => e.PositionId == id);
        }
    }
}
