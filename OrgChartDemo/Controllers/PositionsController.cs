using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using OrgChartDemo.Models.ViewModels;
using OrgChartDemo.Persistence;

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
        /// <returns></returns>
        public IActionResult Index()
        {            
            return View(unitOfWork.Positions.GetPositionsWithMembers());
        }

        /// <summary>
        /// GET: Positions/Details/5.
        /// </summary>
        /// <param name="id">The identifier for a Position.</param>
        /// <returns></returns>
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
        /// <returns></returns>
        public IActionResult Create()
        {
            // TODO : FIX THIS, it needs a ComponentList
            return View();
        }

        /// <summary>
        /// POST: Positions/Create.
        /// </summary>
        /// <param name="form">A <see cref="T:OrgChartDemo.Models.ViewModels.PositionWithComponentListViewModel"/> with certain fields bound on submit</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("PositionName,ParentComponentId,JobTitle,IsManager,IsUnique")] PositionWithComponentListViewModel form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                Position p = new Position
                {
                    ParentComponent = unitOfWork.Components.SingleOrDefault(c => c.ComponentId == form.ParentComponentId),
                    Name = form.PositionName,
                    IsUnique = form.IsUnique,
                    JobTitle = form.JobTitle,
                    IsManager = form.IsManager
                };
            
                // check if a position with the Name provided already exists and reject if so
                if (unitOfWork.Positions.SingleOrDefault(x => x.Name == form.PositionName) != null)
                {                    
                    ViewBag.Message = $"A Position with the name {form.PositionName} already exists. Use a different Name.";
                    return View();
                }
                // check if user is attempting to add "Manager" position to the ParentComponent
                else if (form.IsManager)
                {
                    // check if the Parent Component of the position already has a Position designated as "Manager"
                    if (unitOfWork.Positions.SingleOrDefault(c => c.ParentComponent.ComponentId == form.ParentComponentId && c.IsManager == true) != null) {                        
                        ViewBag.Message = $"{p.ParentComponent.Name} already has a Position designated as Manager. Only one Manager Position is permitted.";
                        return View();
                    }
                }            
                unitOfWork.Positions.Add(p);
                unitOfWork.Complete();
                return RedirectToAction(nameof(Index)); 
            }

        }

        /// <summary>
        /// Positions/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position position = unitOfWork.Positions.SingleOrDefault(x => x.PositionId == id);
            if (position == null)
            {
                return NotFound();
            }            
            return View();
        }

        /// <summary>
        /// POST: Positions/Edit/5
        /// </summary>
        /// <param name="id">The PositionId for the <see cref="T:OrgChartDemo.Models.Position"/> being edited</param>
        /// <param name="form">The <see cref="T:OrgChartDemo.Models.ViewModels.PositionWithComponentListViewModel"/> object to which the POSTed form is Bound</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("PositionId,PositionName,ParentComponentId,JobTitle,IsManager,IsUnique")] PositionWithComponentListViewModel form)
        {
            Position p = unitOfWork.Positions.SingleOrDefault(x => x.PositionId == id);
            Component targetParentComponent = unitOfWork.Components.SingleOrDefault(x => x.ComponentId == form.ParentComponentId);
            if (id != form.PositionId) {
                return NotFound();
            }
            else if (unitOfWork.Positions.Find(x => x.Name == form.PositionName && x.PositionId != id).FirstOrDefault() != null)
            {
                // user is attempting to change the name of the position to a name which already exists
                ViewBag.Message = $"A Position with the name {form.PositionName} already exists. Use a different Name.";                
                return View();
            }
            else if (form.IsManager && unitOfWork.Positions.Find(x => x.ParentComponent.ComponentId == form.ParentComponentId && x.IsManager && x.PositionId != form.PositionId).FirstOrDefault() != null) {
                // user is attempting to elevate a Position to Manager when the ParentComponent already has a Manager                
                ViewBag.Message = $"{targetParentComponent.Name} already has a Position designated as Manager. You can not elevate this Position.";                
                return View();                
            }
            else if (form.IsUnique == true && p.IsUnique == false && p.Members.Count() > 1) {
                // user is attempting to make Position unique when multiple members are assigned
                ViewBag.Message = $"{p.Name} has {p.Members.Count()} current Members. You can't set this Position to Unique with multiple members.";
                
                return View();
            }
            else {
                p.ParentComponent = unitOfWork.Components.Find(c => c.ComponentId == form.ParentComponentId).FirstOrDefault();
                p.Name = form.PositionName;
                p.IsUnique = form.IsUnique;
                p.JobTitle = form.JobTitle;
                p.IsManager = form.IsManager;
                unitOfWork.Complete();
            }
            return RedirectToAction(nameof(Index));
        }
        
        /// <summary>
        /// GET: Positions/Delete/5
        /// </summary>
        /// <param name="id">The PositionId of the <see cref="T:OrgChartDemo.Models.Position"/> being deleted</param>
        /// <returns></returns>
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
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {            
            Position p = unitOfWork.Positions.Get(id);
            unitOfWork.Positions.Remove(p);
            unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Determines if a Position exists with the provided PositionId .
        /// </summary>
        /// <param name="id">The PositionId of the <see cref="T:OrgChartDemo.Models.Position"/></param>
        /// <returns></returns>
        private bool PositionExists(int id)
        {
            return (unitOfWork.Positions.Find(e => e.PositionId == id) != null);
        }
    }
}
