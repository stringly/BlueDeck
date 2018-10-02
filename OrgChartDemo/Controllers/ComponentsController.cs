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
    /// Controller for Position CRUD actions
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
        /// <returns></returns>
        public IActionResult Index()
        {
            return View(unitOfWork.Components.GetAll());
        }

        /// <summary>
        /// GET: Components/Details/5.
        /// </summary>
        /// <param name="id">The identifier for a Component.</param>
        /// <returns></returns>
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
        // TODO: This will need a Component List View Model for the ParentComponent
        /// <summary>
        /// GET: Component/Create.
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            ComponentWithComponentListViewModel vm = new ComponentWithComponentListViewModel(new Component(), unitOfWork.Components.GetAll().ToList());
            return View(vm);
        }

        /// <summary>
        /// POST: Positions/Create.
        /// </summary>
        /// <param name="form">A <see cref="T:OrgChartDemo.Models.ViewModels.ComponentWithComponentListViewModel"/> with certain fields bound on submit</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ComponentId,ParentComponentId,ComponentName,Acronym")] ComponentWithComponentListViewModel form)
        {        
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
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
                    ComponentWithComponentListViewModel vm = new ComponentWithComponentListViewModel(c,)
                }
                unitOfWork.Components.Add(component);
                unitOfWork.Complete();
            }
            return View(component);
        }

        // GET: Components/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var component = await _context.Components.FindAsync(id);
            if (component == null)
            {
                return NotFound();
            }
            return View(component);
        }

        // POST: Components/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ComponentId,Name,Acronym")] Component component)
        {
            if (id != component.ComponentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(component);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComponentExists(component.ComponentId))
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
            return View(component);
        }

        // GET: Components/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var component = await _context.Components
                .FirstOrDefaultAsync(m => m.ComponentId == id);
            if (component == null)
            {
                return NotFound();
            }

            return View(component);
        }

        // POST: Components/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var component = await _context.Components.FindAsync(id);
            _context.Components.Remove(component);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComponentExists(int id)
        {
            return _context.Components.Any(e => e.ComponentId == id);
        }
    }
}
