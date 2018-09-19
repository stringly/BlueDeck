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
    public class PositionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PositionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Positions
        public async Task<IActionResult> Index()
        {

            return View(await _context.Positions
                .Include(m => m.Members)
                .Include(p => p.ParentComponent)
                .ToListAsync());
        }

        // GET: Positions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Positions
                .FirstOrDefaultAsync(m => m.PositionId == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // GET: Positions/Create
        public IActionResult Create()
        {
            PositionWithComponentListViewModel vm = new PositionWithComponentListViewModel(new Position(), _context.Components.ToList());
            return View(vm);
        }

        // POST: Positions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PositionName,ParentComponentId,JobTitle,IsManager,IsUnique")] PositionWithComponentListViewModel form)
        {
            Position p = new Position
            {
                ParentComponent = _context.Components.Where(c => c.ComponentId == form.ParentComponentId).FirstOrDefault(),
                Name = form.PositionName,
                IsUnique = form.IsUnique,
                JobTitle = form.JobTitle,
                IsManager = form.IsManager
            };
            
            // check if a position with the Name provided already exists and reject if so
            if (_context.Positions.Any(x => x.Name == form.PositionName))
            {
                PositionWithComponentListViewModel vm = new PositionWithComponentListViewModel(new Position(), _context.Components.ToList());
                return View(vm);
            }
            // check if user is attempting to add "Manager" position to the ParentComponent
            else if (form.IsManager)
            {
                // check if the Parent Component of the position already has a Position designated as "Manager"
                if (_context.Positions.Where(c => (c.ParentComponent.ComponentId == form.ParentComponentId))
                                     .Any(c => (c.IsManager == true))) {
                    PositionWithComponentListViewModel vm = new PositionWithComponentListViewModel(new Position(), _context.Components.ToList());
                    return View(vm);
                }
            }
            _context.Add(p);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); 
        }

        // GET: Positions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var position = await _context.Positions.FindAsync(id);

            if (position == null)
            {
                return NotFound();
            }
            PositionWithComponentListViewModel vm = new PositionWithComponentListViewModel(new Position(), _context.Components.ToList());
            return View(vm);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {
            if (id != Convert.ToInt32(collection["position.PositionId"])) {
                return NotFound();
            }

            Position p = _context.Positions.Where(x => x.PositionId == id).FirstOrDefault();

            p.ParentComponent = _context.Components.Where(c => c.ComponentId == Convert.ToInt32(collection["Position.ParentComponent.ComponentId"])).FirstOrDefault();
            p.Name = collection["Position.Name"];
            p.IsUnique = collection["Position.IsUnique"].Contains("true");
            p.JobTitle = collection["Position.JobTitle"];
            p.IsManager = collection["Position.IsManager"].Contains("true");
            
            try {
                _context.Update(p);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!PositionExists(p.PositionId)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Positions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Positions
                .FirstOrDefaultAsync(m => m.PositionId == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PositionExists(int id)
        {
            return _context.Positions.Any(e => e.PositionId == id);
        }
    }
}
