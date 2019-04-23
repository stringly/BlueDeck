using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Types;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace OrgChartDemo.Controllers
{
    /// <summary>
    /// Controller for Component CRUD actions
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    public class ComponentsController : Controller
    {
        private IUnitOfWork unitOfWork;
        public int PageSize = 25;

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
        /// <permission>
        /// Any authenticated User can view the Component Index. Auth is handled via Windows. The Components/Index.cshtml view contains
        /// logic that restricts the rendering of hyperlinks to Add/Edit/Delete Components from the rendered Index List.
        /// </permission>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="searchString">The search string.</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        
        public IActionResult Index(string sortOrder, string searchString, int page = 1)
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
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = searchString == null ? unitOfWork.Components.GetAll().Count() : vm.Components.Count()
            };
            ViewBag.Title = "BlueDeck Component Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            vm.Components = vm.Components.Skip((page - 1) * PageSize).Take(PageSize);
            return View(vm);
        }

        /// <summary>
        /// GET: Components/Details/5.
        /// </summary>
        /// <param name="id">The identifier for a Component.</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var component = unitOfWork.Components.GetComponentWithParentComponent(Convert.ToInt32(id));
            if (component == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Component Details";
            ViewBag.ReturnUrl = returnUrl;
            return View(component);
        }
        
        /// <summary>
        /// GET: Component/Create.
        /// </summary>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [Authorize("CanEditComponent")]
        public IActionResult Create(string returnUrl)
        {
            if (User.IsInRole("GlobalAdmin"))
            {
                // if the User is a Global Admin, then they can add a Component as a Child to any of the existing components, 
                // so we build the viewmodel with the full DB component list as Select list options
                ComponentWithComponentListViewModel vm = new ComponentWithComponentListViewModel(new Component(), unitOfWork.Components.GetComponentSelectListItems());
                ViewBag.Title = "Create New Component";
                ViewBag.ReturnUrl = returnUrl;
                return View(vm);
            }
            else if (User.IsInRole("ComponentAdmin"))
            {
                // if the User is not GlobalAdmin, but is a Component Admin, then we restrict the VM to only allow the User to 
                // add a Component to one of the components that they supervise. We retrieve this list from the User's Claims, where a
                // List of ComponentSelectListItems should have been serialized via the ClaimsLoader
                List<ComponentSelectListItem> components = 
                    JsonConvert.DeserializeObject<List<ComponentSelectListItem>>(User.Claims.FirstOrDefault(claim => claim.Type == "CanEditComponents").Value.ToString());
                ComponentWithComponentListViewModel vm = new ComponentWithComponentListViewModel(new Component(), components);
                ViewBag.Title = "Create New Component";
                ViewBag.ReturnUrl = returnUrl;
                return View(vm);
            }
            else
            {
                return Forbid();
            }
        }

        /// <summary>
        /// POST: Components/Create.
        /// </summary>
        /// <param name="form">A <see cref="T:OrgChartDemo.Models.ViewModels.ComponentWithComponentListViewModel"/> with certain fields bound on submit</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ParentComponentId,ComponentName,LineupPosition,Acronym")] ComponentWithComponentListViewModel form, string returnUrl)
        {        
            if (!ModelState.IsValid)
            {
                form.Components = unitOfWork.Components.GetComponentSelectListItems();                
                ViewBag.Title = "Create Component - Corrections Required";
                ViewBag.Status = "Warning!";
                ViewBag.Message = "You must correct the fields indicated.";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            else
            {
                Component c = new Component
                {
                    Name = form.ComponentName,
                    Acronym = form.Acronym,
                    ParentComponent = unitOfWork.Components.SingleOrDefault(x => x.ComponentId == form.ParentComponentId),
                    LineupPosition = form.LineupPosition
                };
                // check if a Component with the Name provided already exists and reject if so
                if (unitOfWork.Components.ComponentNameNotAvailable(c) == true)
                {
                    ViewBag.Title = "Create Component - Name taken";
                    ViewBag.Status = "Warning!";                    
                    ViewBag.Message = $"A Component with the name {form.ComponentName} already exists. Use a different Name.";
                    form.Components = unitOfWork.Components.GetComponentSelectListItems();
                    ViewBag.ReturnUrl = returnUrl;
                    return View(form);
                }
                // add Component to repo via method that controls setting lineup
                unitOfWork.Components.UpdateComponentAndSetLineup(c);
                unitOfWork.Complete();
            }
            TempData["Status"] = "Success!";
            TempData["Message"] = "Component successfully created.";
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Components/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [Authorize("CanEditComponent")]
        public IActionResult Edit(int? id, string returnUrl)
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
            if (User.IsInRole("GlobalAdmin"))
            {
                ComponentWithComponentListViewModel vm = new ComponentWithComponentListViewModel(component, unitOfWork.Components.GetComponentSelectListItems());
                ViewBag.Title = "Edit Component";
                ViewBag.ReturnUrl = returnUrl;
                return View(vm);
            }
            else if (User.IsInRole("ComponentAdmin"))
            {
                List<ComponentSelectListItem> components =
                    JsonConvert.DeserializeObject<List<ComponentSelectListItem>>(User.Claims.FirstOrDefault(claim => claim.Type == "CanEditComponents").Value.ToString());
                ComponentWithComponentListViewModel vm = new ComponentWithComponentListViewModel(component, components);
                ViewBag.Title = "Edit Component";
                ViewBag.ReturnUrl = returnUrl;
                return View(vm);
            }
            else
            {
                return Forbid();
            }
        }

        /// <summary>
        /// POST: Components/Edit/5
        /// </summary>
        /// <param name="id">The ComponentId for the <see cref="T:OrgChartDemo.Models.Component"/> being edited</param>
        /// <param name="form">The <see cref="T:OrgChartDemo.Models.ViewModels.ComponentWithComponentListViewModel"/> object to which the POSTed form is Bound</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ComponentId,ParentComponentId,ComponentName,LineupPosition,Acronym")] ComponentWithComponentListViewModel form, string returnUrl )
        {
            Component c = unitOfWork.Components.SingleOrDefault(x => x.ComponentId == id);
            Component targetParentComponent = unitOfWork.Components.SingleOrDefault(x => x.ComponentId == form.ParentComponentId);
            if (id != form.ComponentId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                if (User.IsInRole("GlobalAdmin"))
                {
                    form.Components = unitOfWork.Components.GetComponentSelectListItems();
                }
                else
                {
                    form.Components =
                        JsonConvert.DeserializeObject<List<ComponentSelectListItem>>(
                            User.Claims.FirstOrDefault(
                                claim => claim.Type == "CanEditComponents")
                                .Value
                                .ToString());
                }
                ViewBag.Title = "Edit Component - Corrections Required";
                ViewBag.Status = "Warning!";
                ViewBag.Message = "You must correct the fields indicated.";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            else if (unitOfWork.Components.SingleOrDefault(x => x.Name == form.ComponentName && x.ComponentId != c.ComponentId) != null)
            {
                ViewBag.Message = $"A Component with the name {form.ComponentName} already exists. Use a different Name.";
                if (User.IsInRole("GlobalAdmin"))
                {
                    form.Components = unitOfWork.Components.GetComponentSelectListItems();
                }
                else
                {
                    form.Components =
                        JsonConvert.DeserializeObject<List<ComponentSelectListItem>>(
                            User.Claims.FirstOrDefault(
                                claim => claim.Type == "CanEditComponents")
                                .Value
                                .ToString());
                }
                ViewBag.Title = "Edit Component - Corrections Required";
                ViewBag.Status = "Warning!";
                ViewBag.Message = "You must correct the fields indicated.";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            else { 
                try
                {
                    // Note: the repo method to control setting the ComponentLineup for sibling components requires that the component
                    // that we are editing be passed as a "new" component because the repo method needs to be able to refer to the existing LineupPosition
                    // of the edited component prior to it being changed
                    Component componentToEdit = new Component()
                    {
                        ComponentId = c.ComponentId,
                        Acronym = form.Acronym,
                        Name = form.ComponentName,
                        LineupPosition = form.LineupPosition,
                        ParentComponent = targetParentComponent
                    };
                    unitOfWork.Components.UpdateComponentAndSetLineup(componentToEdit);
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
                TempData["Status"] = "Success!";
                TempData["Message"] = "Component updated successfully.";
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }                
            }
        }

        /// <summary>
        /// GET: Components/Delete/5
        /// </summary>
        /// <param name="id">The ComponentId of the <see cref="T:OrgChartDemo.Models.Component"/> being deleted</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [Authorize("CanEditComponent")]
        public IActionResult Delete(int? id, string returnUrl)
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
            if(component.ChildComponents.Count() > 0)
            {
                ViewBag.Message = $"This Component currently has {component.ChildComponents.Count()} child Components. You cannot delete a Component that has active child Components.";
                ViewBag.Status = "Warning!";                
            }
            else if (component.Positions.Count() > 0)
            {
                int totalMembers = 0;
                foreach (Position p in component.Positions)
                {
                    totalMembers += p.Members.Count();
                }
                ViewBag.Message = $"This Component includes {component.Positions.Count()} Positions with a total of {totalMembers} Members.\n"
                                        + "Deleting this Component will also delete all of it's assigned Positions and reassign all Members to 'Unassigned.'";
                ViewBag.Status = "Warning!";
            }
            ViewBag.Title = "Confirm Delete?";
            ViewBag.ReturnUrl = returnUrl;
            return View(component);
        }

        /// <summary>
        /// POST: Components/Delete/5
        /// </summary>
        /// <param name="id">The ComponentId of the <see cref="T:OrgChartDemo.Models.Component"/> being deleted</param>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            unitOfWork.Components.RemoveComponent(id);
            unitOfWork.Complete();            
            TempData["Status"] = "Success!";
            TempData["Message"] = "Component successfully deleted.";
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
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
