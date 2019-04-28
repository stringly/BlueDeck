using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using OrgChartDemo.Models.ViewModels;
using OrgChartDemo.Models.Types;
using OrgChartDemo.Persistence;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace OrgChartDemo.Controllers
{
    /// <summary>
    /// Controller for Position CRUD actions
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    public class PositionsController : Controller
    {
        private IUnitOfWork unitOfWork;
        public int PageSize = 25;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Controllers.PositionsController"/> class.
        /// </summary>
        /// <param name="unit"><see cref="T:OrgChartDemo.Persistence.UnitOfWork"/>.</param>
        public PositionsController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

       
        /// <summary>
        /// GET: Positions
        /// </summary>
        /// <remarks>
        /// This View requires an <see cref="T:IEnumerable{T}"/> list of <see cref="T:OrgChartDemo.Models.ViewModels.PositionWithMemberCountItem"/>
        /// </remarks>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Index(string sortOrder, string searchString, int page = 1)
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
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = searchString == null ? unitOfWork.Positions.GetAll().Count() : vm.Positions.Count()
            };
            ViewBag.Title = "BlueDeck Positions Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            vm.Positions = vm.Positions.Skip((page - 1) * PageSize).Take(PageSize);
            return View(vm);
        }

        /// <summary>
        /// GET: Positions/Details/5.
        /// </summary>
        /// <param name="id">The identifier for a Position.</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = unitOfWork.Positions.GetPositionWithParentComponent(Convert.ToInt32(id));
                
            if (position == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Position Details";
            ViewBag.ReturnUrl = returnUrl;
            return View(position);
        }

        /// <summary>
        /// GET: Positions/Create.
        /// </summary>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [Authorize("CanEditPosition")]
        public IActionResult Create(string returnUrl)
        {
            PositionWithComponentListViewModel vm = new PositionWithComponentListViewModel(new Position());
            if (User.IsInRole("GlobalAdmin"))
            {
                vm.Components = unitOfWork.Components.GetComponentSelectListItems();
                vm.AvailableMembers = unitOfWork.Members.GetAllMemberSelectListItems().ToList();
            }
            else if (User.IsInRole("ComponentAdmin"))
            {
                vm.Components = JsonConvert.DeserializeObject<List<ComponentSelectListItem>>(User.Claims.FirstOrDefault(claim => claim.Type == "CanEditComponents").Value.ToString());
                vm.AvailableMembers = JsonConvert.DeserializeObject<List<MemberSelectListItem>>(User.Claims.FirstOrDefault(claim => claim.Type == "CanEditUsers").Value.ToString());
            }
            else
            {
                return Forbid();
            }
            ViewBag.Title = "Create New Position";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }

        /// <summary>
        /// POST: Positions/Create.
        /// </summary>
        /// <param name="form">A <see cref="T:OrgChartDemo.Models.ViewModels.PositionWithComponentListViewModel"/> with certain fields bound on submit</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            [Bind("PositionName," +
            "LineupPosition," +
            "ParentComponentId," +
            "JobTitle," +
            "Callsign," +
            "IsManager," +
            "IsUnique," +
            "CurrentMembers")] PositionWithComponentListViewModel form, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Create Position: Corrections required";
                ViewBag.Status = "Warning!";
                ViewBag.Message = "You must correct the fields indicated";
                if (User.IsInRole("GlobalAdmin"))
                {
                    form.Components = unitOfWork.Components.GetComponentSelectListItems();
                    form.AvailableMembers = unitOfWork.Members.GetAllMemberSelectListItems().ToList();
                }
                else if(User.IsInRole("ComponentAdmin"))
                {
                    form.Components = JsonConvert.DeserializeObject<List<ComponentSelectListItem>>(User.Claims.FirstOrDefault(claim => claim.Type == "CanEditComponents").Value.ToString());
                    form.AvailableMembers = JsonConvert.DeserializeObject<List<MemberSelectListItem>>(User.Claims.FirstOrDefault(claim => claim.Type == "CanEditUsers").Value.ToString());
                }
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }

            int errors = 0;
            Component targetParentComponent = unitOfWork.Components.SingleOrDefault(c => c.ComponentId == form.ParentComponentId);
            Position p = new Position {
                ParentComponent = targetParentComponent,
                Name = form.PositionName,
                IsUnique = form.IsUnique,
                JobTitle = form.JobTitle,
                IsManager = form.IsManager,
                LineupPosition = form.LineupPosition,
                Callsign = form.Callsign.ToUpper(),
                CreatedDate = DateTime.Now,
                LastModified = DateTime.Now,
                CreatorId = Convert.ToInt32(User.Claims.FirstOrDefault(claim => claim.Type == "MemberId").Value),
                LastModifiedById = Convert.ToInt32(User.Claims.FirstOrDefault(claim => claim.Type == "MemberId").Value)
                };
            if (form.CurrentMembers != null)
            {
                p.Members = new List<Member>();
                foreach (MemberLineupItem mli in form.CurrentMembers)
                {
                    Member m = unitOfWork.Members.Get(mli.MemberId);
                    p.Members.Add(m);
                }
            }
            if (unitOfWork.Positions.SingleOrDefault(x => x.Name == form.PositionName && x.ParentComponentId == form.ParentComponentId) != null) { 
                ViewBag.Message = $"A Position with the name {form.PositionName} already exists. Use a different Name.\n";
                errors++;
            }
            else if (unitOfWork.Positions.SingleOrDefault(x => x.ParentComponentId == form.ParentComponentId && x.Callsign == form.Callsign) != null ) {
                errors++;
                ViewBag.Message = $"The callsign '{p.Callsign}' is in use. Choose another.";
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
                TempData["Status"] = "Success!";
                TempData["Message"] = "Position successfully created.";
                if(returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(Index)); 
                }                
            }
            else {
                form.Components = unitOfWork.Components.GetComponentSelectListItems();
                ViewBag.Title = "New Position: Corrections Required";
                ViewBag.Status = "Warning!";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
        }

        /// <summary>
        /// Positions/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [Authorize("CanEditPosition")]
        public IActionResult Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position position = unitOfWork.Positions.GetPositionAndAllCurrentMembers(Convert.ToInt32(id));
            if (position == null)
            {
                return NotFound();
            }
            PositionWithComponentListViewModel vm = new PositionWithComponentListViewModel(position);
            if (User.IsInRole("GlobalAdmin"))
            {
                vm.Components = unitOfWork.Components.GetComponentSelectListItems();
                vm.AvailableMembers = unitOfWork.Members.GetAllMemberSelectListItems().ToList();
            }
            else if (User.IsInRole("ComponentAdmin"))
            {
                vm.Components = JsonConvert.DeserializeObject<List<ComponentSelectListItem>>(User.Claims.FirstOrDefault(claim => claim.Type == "CanEditComponents").Value.ToString());
                vm.AvailableMembers = JsonConvert.DeserializeObject<List<MemberSelectListItem>>(User.Claims.FirstOrDefault(claim => claim.Type == "CanEditUsers").Value.ToString());
            }
            else
            {
                return Forbid();
            }
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Title = "Edit Position";
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
        public IActionResult Edit(int id, 
            [Bind("PositionId," +
            "PositionName," +
            "LineupPosition," +
            "ParentComponentId," +
            "JobTitle," +
            "Callsign," +
            "IsManager," +
            "IsUnique," +
            "CurrentMembers" +
            "Creator," +
            "CreatedDate," +
            "CreatedById," + 
            "LastModifiedBy," +
            "LastModified," +
            "LastModifiedById")] PositionWithComponentListViewModel form, string returnUrl)
        {
            int errors = 0;
            Component targetParentComponent = unitOfWork.Components.Find(c => c.ComponentId == form.ParentComponentId).FirstOrDefault();
            Position p = new Position();
            
            if (!ModelState.IsValid) {
                errors++;
                ViewBag.Message = "You must correct the fields indicated.";
            }
            else {            

                if (id != form.PositionId) {
                    return NotFound();
                }
                else if (unitOfWork.Positions.Find(x => x.Name == form.PositionName && x.ParentComponentId == form.ParentComponentId && x.PositionId != id).FirstOrDefault() != null)
                {
                    // user is attempting to change the name of the position to a name which already exists                    
                    ViewBag.Message = $"A Position with the name {form.PositionName} already exists in the selected parent Component. Use a different Name.\n";
                    errors++;
                }
                else if (form.Callsign != null)
                {
                    if(unitOfWork.Positions.SingleOrDefault(x => x.Callsign == form.Callsign.ToUpper() && x.ParentComponentId == form.ParentComponentId && x.PositionId != form.PositionId) != null)
                    {
                        errors++;                    
                        ViewBag.Message = $"The callsign {form.Callsign} is in use by another position in the selected parent Component. Choose another.";
                    }
                    
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
                p.Callsign = form?.Callsign?.ToUpper() ?? null;
                p.IsManager = form.IsManager;
                p.LineupPosition = form.LineupPosition;
                p.LastModifiedById = Convert.ToInt32(User.Claims.FirstOrDefault(claim => claim.Type == "MemberId").Value);
                p.LastModified = DateTime.Now;
                if (form.CurrentMembers != null)
                {
                    List<Member> formMembers = new List<Member>();
                    foreach (MemberLineupItem mli in form.CurrentMembers)
                    {
                        Member m = unitOfWork.Members.Get(mli.MemberId);
                        formMembers.Add(m);
                    }
                    p.Members = formMembers;
                }
                else
                {
                    p.Members = new List<Member>();
                }
                unitOfWork.Positions.UpdatePositionAndSetLineup(p);
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Position successfully updated";
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }                
            } else
            {
                form.Components = unitOfWork.Components.GetComponentSelectListItems();
                ViewBag.Title = "Edit Position - Corrections Required";
                ViewBag.Status = "Warning!";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            
        }
        
        /// <summary>
        /// GET: Positions/Delete/5
        /// </summary>
        /// <param name="id">The PositionId of the <see cref="T:OrgChartDemo.Models.Position"/> being deleted</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [Authorize("CanEditPosition")]
        public IActionResult Delete(int? id, string returnUrl)
        {
            // TODO: Warn or Prevent User from Deleting a Position with assigned Members? Or auto-reassign members to the General Pool?
            if (id == null)
            {
                return NotFound();
            }

            var position = unitOfWork.Positions.GetPositionWithParentComponent(Convert.ToInt32(id));
            if (position == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Confirm - Delete Position?";
            ViewBag.ReturnUrl = returnUrl;
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
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            unitOfWork.Positions.RemovePositionAndReassignMembers(id);
            unitOfWork.Complete();
            TempData["Status"] = "Success!";
            TempData["Message"] = "Position successfully deleted";
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
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

        public IActionResult AssignMember(string addOrRemove, int PositionId, int MemberId)
        {
            Member m = unitOfWork.Members.Get(MemberId);
            switch (addOrRemove)
            {
                case "add":
                    Position p = unitOfWork.Positions.GetPositionAndAllCurrentMembers(PositionId);
                    if(p.IsUnique == true)
                    {
                        foreach(Member cm in p.Members)
                        {
                            cm.PositionId = unitOfWork.Positions.Find(x => x.Name == "Unassigned").FirstOrDefault().PositionId;
                        }
                    }
                    m.PositionId = PositionId;
                    break;
                case "remove":
                    m.PositionId = unitOfWork.Positions.Find(x => x.Name == "Unassigned").FirstOrDefault().PositionId;
                    break;
            }
            unitOfWork.Complete();
            return RedirectToAction("GetMemberLineupViewComponent", new { positionId = PositionId});
        }

    }
}
