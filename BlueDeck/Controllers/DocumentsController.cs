using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueDeck.Models.DocGenerators;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlueDeck.Controllers
{
    /// <summary>
    /// Controller that generates OpenXML Documents
    /// </summary>
    /// <seealso cref="Controller" />
    public class DocumentsController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsController"/> class.
        /// </summary>
        /// <param name="_unit">An injected <see cref="IUnitOfWork"/> obtained from the services middleware.</param>
        public DocumentsController(IUnitOfWork _unit)
        {
            unitOfWork = _unit;
        }

        /// <summary>
        /// GET: Documents/Index
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Documents/Index")]       
        public IActionResult Index()
        {
            DocumentsIndexViewModel vm = new DocumentsIndexViewModel(unitOfWork.Components.GetComponentSelectListItems());
            return View(vm);
        }

        /// <summary>
        /// Lineups the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Documents/Lineup/{id:int}")]
        public IActionResult Lineup(int id)
        {                        
            LineupGeneratorViewModel vm = unitOfWork.Components.GetLineupGeneratorViewModel(id);
            return View(vm);           
            
        }

        /// <summary>
        /// Downloads an Component roster for the Component with the provided identity.
        /// </summary>
        /// <param name="id">The identity of the Component.</param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("Documents/Lineup")]
        //public IActionResult Lineup([Bind] LineupGeneratorViewModel form)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        LineupGenerator gen = new LineupGenerator(form);
        //        string fileName = $"Lineup {form.ComponentName} {DateTime.Now.ToString("MM'-'dd'-'yy")}.docx";
        //        return File(gen.Generate(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
        //    }
        //    else
        //    {
        //        // TODO: repopulate Select Lists
        //        form.Vehicles = unitOfWork.Vehicles.GetVehicleSelectListItems();
        //        return View(form);
        //    }
        //}

        /// <summary>
        /// Downloads an alphabetical roster for the Component with the provided identity.
        /// </summary>
        /// <param name="id">The identity of the Component.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Documents/DownloadAlphaRoster/{id:int}")]
        public IActionResult DownloadAlphaRoster(int id)
        {
            AlphaRosterGenerator gen = new AlphaRosterGenerator();
            gen.Members = unitOfWork.Components.GetMembersRosterForComponentId(id);
            gen.ComponentName = unitOfWork.Components.Get(id).Name;
            string fileName = $"{unitOfWork.Components.Get(id).Name} Alpha Roster {DateTime.Now.ToString("MM'-'dd'-'yy")}.docx";

            return File(gen.Generate(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
        }

        /// <summary>
        /// Downloads an Component roster for the Component with the provided identity.
        /// </summary>
        /// <param name="id">The identity of the Component.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Documents/DownloadComponentRoster/{id:int}")]
        public IActionResult DownloadComponentRoster(int id)
        {
            TraditionalRosterGenerator gen = new TraditionalRosterGenerator(unitOfWork.Components.GetComponentsAndChildrenWithParentSP(id));
            //ComponentRosterGenerator gen = new ComponentRosterGenerator(unitOfWork.Components.GetComponentsAndChildrenWithParentSP(id));
            string fileName = $"{unitOfWork.Components.Get(id).Name} Roster {DateTime.Now.ToString("MM'-'dd'-'yy")}.docx";
            return File(gen.Generate(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
        }

        /// <summary>
        /// Downloads an organization chart for the Component with the provided id.
        /// </summary>
        /// <param name="id">The identity of the Component.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Documents/DownloadOrganizationChart/{id:int}")]
        public IActionResult DownloadOrganizationChart(int id)
        {
            OrgChartGenerator gen = new OrgChartGenerator(unitOfWork.Components.GetOrgChartComponentsWithMembersNoMarkup(id));
            string fileName = $"{unitOfWork.Components.Get(id).Name} Organization Chart {DateTime.Now.ToString("MM'-'dd'-'yy")}.docx";
            return File(gen.Generate(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
        }
    }
}