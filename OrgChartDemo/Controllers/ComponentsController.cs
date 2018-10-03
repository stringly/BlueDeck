using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;

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
        /// This View requires an <see cref="T:IEnumerable{T}"/> list of <see cref="T:OrgChartDemo.Models.Component"/>
        /// </remarks>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Index()
        {
            return View(unitOfWork.Components.GetAll());
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
        public IActionResult Create([Bind("ParentComponentId,ComponentName,Acronym")] ComponentWithComponentListViewModel form)
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
                    // TODO: How will this handle a null ParentComponentId?
                    ParentComponent = unitOfWork.Components.SingleOrDefault(x => x.ComponentId == form.ParentComponentId),
                };
                // check if a Component with the Name provided already exists and reject if so
                if (unitOfWork.Components.SingleOrDefault(x => x.Name == form.ComponentName) != null)
                {
                    ViewBag.Messaage = $"A Component with the name {form.ComponentName} already exists. Use a different Name.";
                    ComponentWithComponentListViewModel vm = new ComponentWithComponentListViewModel(c, unitOfWork.Components.GetAll().ToList());
                }
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
        public IActionResult Edit(int id, [Bind("ParentComponentId,ComponentName,Acronym")] ComponentWithComponentListViewModel form )
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
            // TODO: Warn or Prevent User from Deleting a Component with assigned Positions? Or auto-reassign members to the General Pool?
            if (id == null)
            {
                return NotFound();
            }
            Component component = unitOfWork.Components.SingleOrDefault(c => c.ComponentId == id);
            if (component == null)
            {
                return NotFound();
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
                    Position unassigned = unitOfWork.Positions.Find(x => x.Name == "Unassigned").FirstOrDefault();
                    foreach (Member m in p.Members)
                    {
                        m.Position = unassigned;
                    }
                }
                unitOfWork.Positions.Remove(p);               
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
    }
}
