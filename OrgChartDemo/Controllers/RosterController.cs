using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using OrgChartDemo.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using OrgChartDemo.Models.Types;
using System;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace OrgChartDemo.Controllers
{
    [Authorize("CanEditComponent")]
    public class RosterController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Controllers.RosterController"/> class.
        /// </summary>
        /// <param name="unit">An <see cref="T:OrgChartDemo.Models.IUnitOfWork"/></param>
        public RosterController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }


        public IActionResult Index(int? id)
        {
            var vm = new RosterManagerViewModel();
            if (User.IsInRole("GlobalAdmin"))
            {
                vm.Components = unitOfWork.Components.GetComponentSelectListItems();
            }
            else if (User.IsInRole("ComponentAdmin"))
            {
                vm.Components = JsonConvert.DeserializeObject<List<ComponentSelectListItem>>(User.Claims.FirstOrDefault(claim => claim.Type == "CanEditComponents").Value.ToString());                
            }
            else
            {
                return Forbid();
            }
            
            if(id != null)
            {
                vm.SelectedComponentId = Convert.ToInt32(id);
            }
            ViewBag.Title = "Roster Manager";
            return View(vm);
        }

        // TODO: Is this Roster/GetComponents deprecated?
        /// <summary>
        /// Gets the components. (async, JSON result from the GetOrgChart JQuery Library)
        /// </summary>
        /// <remarks>
        /// This method is used to 
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetComponents(int componentId)
        {
            // TODO: update the repo method to retrieve data reequired for this view            
            List<Component> result = unitOfWork.Components.GetComponentAndChildren(componentId, new List<Component>());
            return Json(result);
        }

        // Drag-and-Drop Functions
        /// <summary>
        /// Reassigns the member to a new position.
        /// </summary>
        /// <remarks>
        /// This method is invoked via the View's Javascript when a member is Dragged/Dropped into a new position in the RosterManager
        /// </remarks>
        /// <param name="memberId">The MemberId of the member being reassigned.</param>
        /// <param name="positionId">The PositionId of the new position to which the member is to be assigned</param>
        /// <param name="selectedComponentId">The selected component identifier.</param>
        /// <returns></returns>
        public JsonResult ReassignMember(int memberId, int positionId, int selectedComponentId)
        {                   
            // ensure member/position isn't 0
            if (memberId != 0 && positionId != 0)
            {
                // pull Member from Repo
                Member m = unitOfWork.Members.GetMemberWithPosition(memberId);
                // pull the Member's current Position
                Position oldPosition = unitOfWork.Positions.GetPositionWithParentComponent(m.Position.PositionId);
                // pull the new Position to which we are going to assign the member
                Position newPosition = unitOfWork.Positions.GetPositionWithParentComponent(positionId);
                // check that we've successfully retrieved the Member and the new Position
                if (m != null && newPosition != null)
                {
                    // Reassign the member
                    m.Position = newPosition;
                    unitOfWork.Complete();
                }
                // next, we check if we have a condition that requires a refresh of the RosterManager View Component

                // if the member is moved to a position outside of his original position's component, we need to update the demographics table
                // here we check for that condition AND if the new position is Unique... if these conditions are met, we only want to return the 
                // updated demotables to the client. If the
                if (newPosition.ParentComponent.ComponentId != oldPosition.ParentComponent.ComponentId && newPosition.IsUnique == true)
                {
                    /*Dictionary<string, string> demoTableDictionary = new Dictionary<string, string>();     
                    List<Component> componentList = unitOfWork.Components.GetComponentAndChildren(selectedComponentId, new List<Component>());
                    RosterManagerViewComponentViewModel vm = new RosterManagerViewComponentViewModel(componentList);
                    demoTableDictionary = vm.GetDemoTableDictionaryForAllComponents();
                    return Json(new { Status = "Success", DemoDictionary = demoTableDictionary});*/

                    // when I implemented the "AssignMember" modal, if a member from outside the RosterManager's component scope is assigned to a "unique" position, the 
                    // RosterManager would'nt refresh... only the demotable, so the result would be that a new "member" draggable wouldn't be rendered.
                    // Considering that the above is doing 90% of all of the work of refreshing the RosterManager anyways, I figure fuck it, refresh the whole damn thing.
                    return Json(new { Status = "RefreshRosterManager" });
                }
                // if thhe above condition isn't true, then we need to check if we have added the member to a "non-unique" position and re-render 
                // the RosterManager ViewComponent so that an empty "insertable" can be appended to the component. 
                else if (newPosition.IsUnique == false || oldPosition.IsUnique == false)
                {
                    // return a status object to trigger the RosterManager refresh via $.get on the Client
                    return Json(new { Status = "RefreshRosterManager" });
                }
            }
            // empty object return if no refresh is required. This should only happen when a member is re-assigned to a unique position 
            // with the same ParentComponent as his original Position
            return Json(new {});
        }

        public JsonResult SwapMemberPositions(int dragMemberId, int dropMemberId)
        {
            Member dragMember = unitOfWork.Members.GetMemberWithPosition(dragMemberId);
            Member dropMember = unitOfWork.Members.GetMemberWithPosition(dropMemberId);
            Position dragPosition = dragMember.Position;
            Position dropPosition = dropMember.Position;
            dragMember.Position = dropPosition;
            dropMember.Position = dragPosition;
            unitOfWork.Complete();
        
            return Json(new { success = true });
            
        }

        // RosterManagerViewComponent
        /// <summary>
        /// Gets the RosterManager ViewComponent.
        /// </summary>
        /// <param name="componentId">The ComponentId of the top-level component</param>
        /// <returns></returns>
        public IActionResult GetRosterViewComponent(int componentId){
            //List<Component> result = unitOfWork.Components.GetComponentAndChildren(componentId, new List<Component>());
            List<Component> result = unitOfWork.Components.GetComponentsAndChildrenSP(componentId);
            return ViewComponent("RosterManager", result.OrderBy(x => x.ComponentId).ToList());    
        }

        // ReassignEmployeeModalViewComponent
        /// <summary>
        /// Gets the ReassignEmployee ViewComponent.
        /// </summary>
        /// <param name="memberId">The MemberId of the Member being reassigned.</param>
        /// <param name="selectedComponentId">The ComponentId of the current top-level component shown in the RosterManager.</param>
        /// <returns></returns>
        public IActionResult GetReassignMemberViewComponent(int memberId, int selectedComponentId)
        {
            Member member = unitOfWork.Members.GetMemberWithPosition(memberId);
            IEnumerable<PositionSelectListItem> positions = unitOfWork.Positions.GetUnoccupiedAndNonUniquePositionSelectListItems();
            return ViewComponent("ReassignEmployeeModal", new { m = member, p = positions, s = selectedComponentId});
        }
               
        /// <summary>
        /// Reassigns the member via POST from the ReassignMemberModal ViewComponent.
        /// </summary>
        /// <param name="form">A POSTed <see cref="T:OrgChartDemo.Models.ViewModels.ReassignEmployeeModalViewComponentViewModel"></see></param>
        [HttpPost]
        public void ReassignMemberViaModal([Bind("PositionId","MemberId","SelectedComponentId")] ReassignEmployeeModalViewComponentViewModel form)
        {            
                Member m = unitOfWork.Members.SingleOrDefault(x => x.MemberId == form.MemberId);
                Position p = unitOfWork.Positions.SingleOrDefault(x => x.PositionId == form.PositionId);
                m.Position = p;
                unitOfWork.Complete();                        
        }

        /* 
         * AddPositionToComponentModalViewComponent
         * This modal handles Adding, Editing, and Deleting a Position via the RosterManager UI
        */
        /// <summary>
        /// Gets the AddPositionToComponentViewComponent.
        /// </summary>
        /// <param name="componentId">The ComponentId of the Component to which a Position is being added</param>
        /// <returns>A <see cref="T:OrgChartDemo.ViewComponents.AddPositionToComponentViewComponent"/></returns>
        public IActionResult GetAddPositionToComponentViewComponent(int componentId, int? positionId = null)
        { 
            if (positionId != null)
            {
                Position position = unitOfWork.Positions.GetPositionWithParentComponent(Convert.ToInt32(positionId));
                if (User.IsInRole("GlobalAdmin"))
                {
                    List<ComponentSelectListItem> componentList = unitOfWork.Components.GetComponentSelectListItems();
                    AddPositionToComponentViewComponentViewModel viewModel = new AddPositionToComponentViewComponentViewModel(position, componentList);
                    return ViewComponent("AddPositionToComponent", new { vm = viewModel });
                }
                else if (User.IsInRole("ComponentAdmin"))
                {
                    List<ComponentSelectListItem> componentList = JsonConvert.DeserializeObject<List<ComponentSelectListItem>>(
                        User.Claims.FirstOrDefault(
                            claim => claim.Type == "CanEditComponents")
                            .Value
                            .ToString());
                    AddPositionToComponentViewComponentViewModel viewModel = new AddPositionToComponentViewComponentViewModel(position, componentList);
                    return ViewComponent("AddPositionToComponent", new { vm = viewModel });
                }
                else
                {
                    return Forbid();
                }
            }
            else
            {
                Component parent = unitOfWork.Components.Get(componentId);
                AddPositionToComponentViewComponentViewModel viewModel = new AddPositionToComponentViewComponentViewModel(parent);
                return ViewComponent("AddPositionToComponent", new { vm = viewModel });
            }
            
        }

        /// <summary>
        /// Adds a new Position to a Component via POST from the AddPositionToComponentViewComponent.
        /// </summary>
        /// <param name="form">The POSTed <see cref="T:OrgChartDemo.Models.ViewModels.AddPositionToComponentViewComponentViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddPositionToComponent([Bind("PositionId,ParentComponentId,LineupPosition,Callsign,PositionName,JobTitle,IsManager,IsUnique")] AddPositionToComponentViewComponentViewModel form)
        {
            // populate the Form object's ParentComponent property with it's ParentComponent
            form.ParentComponent = unitOfWork.Components.GetComponentWithChildren(Convert.ToInt32(form.ParentComponentId));
            // pull a list of all Positions in the Repo to use to check name conflict
            IEnumerable<Position> allPositions = unitOfWork.Positions.GetAll();
            
            if (form.Callsign != null)
            {
                form.Callsign = form.Callsign.ToUpper();
            }
            else
            {
                form.Callsign = "NONE";
            }
            // check Model validation first
            if (ModelState.IsValid)
            {
                // if ModelValidation passes, create a counter to store count of errors for other validation rules
                var errors = 0;                  
                // check to make sure submitted new position name doesn't conflict with an existing position
                foreach(Position p in allPositions)
                {
                    // check for conflict in Position Name
                    if (p.Name == form.PositionName && p.PositionId != form.PositionId)
                    {
                        errors++;
                        ViewBag.Message = $"A Position with the name {form.PositionName} already exists. Use a different Name.\n";                            
                    }
                    else if (form.Callsign != "NONE")
                    {
                        if (p.Callsign == form.Callsign && p.PositionId != form.PositionId)
                        {
                            errors++;
                            ViewBag.Message = $"The callsign '{form.Callsign}' is in use by {p.Name}. Choose another.";
                        }
                    }                    
                }
                // check for conflict in "IsManager" for all Positions in the ParentComponent
                foreach(Position p in form.ParentComponent.Positions)
                {   
                    if (form.IsManager == true && p.PositionId != form.PositionId)
                    {
                        if (p.IsManager == true)
                        {
                            errors++;
                            ViewBag.Message +=  $"{p.ParentComponent.Name} already has a Position designated as Manager. Only one Manager Position is permitted.\n";
                        }                        
                    }
                }
                
                
                // no validation errors, safe to add position
                if (errors == 0)
                {
                    // add the position via unitOfWork
                    if (form.PositionId == null)
                    {
                        Position p = new Position(){
                            ParentComponent = form.ParentComponent,
                            Name = form.PositionName,
                            JobTitle = form.JobTitle,
                            IsManager = form.IsManager,
                            IsUnique = form.IsUnique,
                            LineupPosition = form.LineupPosition,
                            Callsign = form.Callsign
                        };
                        unitOfWork.Positions.UpdatePositionAndSetLineup(p);
                        unitOfWork.Complete();
                    }
                    else
                    {
                        Position p = new Position() {
                            PositionId = Convert.ToInt32(form.PositionId),
                            ParentComponent = form.ParentComponent,
                            Name = form.PositionName,
                            JobTitle = form.JobTitle,
                            IsManager = form.IsManager,
                            IsUnique = form.IsUnique,
                            LineupPosition = form.LineupPosition,
                            Callsign = form.Callsign
                        };
                        unitOfWork.Positions.UpdatePositionAndSetLineup(p);
                        unitOfWork.Complete();
                    }                                        
                    // return a JSON object to the Ajax POST so that it can hide the Modal
                    return Json(new { success = true });
                }
                // validation errors, return the ViewModel with the ViewBag.Message showing
                else 
                {
                    form.ComponentList = unitOfWork.Components.GetComponentSelectListItems();
                    return ViewComponent("AddPositionToComponent", form);
                }
            }
            else // ModelState invalid
            {   
                form.ComponentList = unitOfWork.Components.GetComponentSelectListItems();
                return ViewComponent("AddPositionToComponent", form);
            }
            
        }

        [HttpPost]
        public IActionResult DeletePosition(int PositionId)
        {
            unitOfWork.Positions.RemovePositionAndReassignMembers(PositionId);
            unitOfWork.Complete();
            return Json(new { success = true });
        }
        // EditEmployeeModalViewModalViewComponent
        public IActionResult GetEditEmployeeModalViewComponent(int memberId)
        {
            Member m = unitOfWork.Members.GetMemberWithDemographicsAndDutyStatus(memberId);            
            MemberAddEditViewModel vm = new MemberAddEditViewModel(m, unitOfWork.MemberRanks.GetMemberRankSelectListItems(),
                unitOfWork.MemberGenders.GetMemberGenderSelectListItems(),
                unitOfWork.MemberRaces.GetMemberRaceSelectListItems(),
                unitOfWork.MemberDutyStatus.GetMemberDutyStatusSelectListItems(),
                unitOfWork.PhoneNumberTypes.GetPhoneNumberTypeSelectListItems());
            return ViewComponent("EditMemberModal", vm);
                
        }

        
        [HttpPost]
        public IActionResult EditMemberModal([Bind("MemberId,MemberRank,MemberGender,MemberRace,FirstName,LastName,MiddleName,IdNumber,DutyStatusId,Email,ContactNumbers")] MemberAddEditViewModel form)
        {
            
            if (ModelState.IsValid)
            {
                unitOfWork.Members.UpdateMember(form);
                unitOfWork.Complete();
                return Json(new { success = true });
            }
            else
            {
                Member m = unitOfWork.Members.GetMemberWithDemographicsAndDutyStatus(Convert.ToInt32(form.MemberId));
                MemberAddEditViewModel vm = new MemberAddEditViewModel(m, unitOfWork.MemberRanks.GetMemberRankSelectListItems(),
                unitOfWork.MemberGenders.GetMemberGenderSelectListItems(),
                unitOfWork.MemberRaces.GetMemberRaceSelectListItems(),
                unitOfWork.MemberDutyStatus.GetMemberDutyStatusSelectListItems(),
                unitOfWork.PhoneNumberTypes.GetPhoneNumberTypeSelectListItems());
                return ViewComponent("EditMemberModal", vm);
            }
        }

        // ChangeEmployeeDutyStatusModalViewComponent

        public IActionResult GetChangeEmployeeStatusModalViewComponent(int memberId)
        {
            Member m = unitOfWork.Members.GetMemberWithPosition(memberId);
            if (m != null)
            {
                List<MemberDutyStatusSelectListItem> statusList = unitOfWork.MemberDutyStatus.GetMemberDutyStatusSelectListItems();
                ChangeEmployeeStatusModalViewComponentViewModel vm = new ChangeEmployeeStatusModalViewComponentViewModel(m, statusList);
                return ViewComponent("ChangeEmployeeStatusModal", vm);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult ChangeEmployeeStatus([Bind("MemberId,DutyStatus,ParentComponentId")] ChangeEmployeeStatusModalViewComponentViewModel form)
        {
            // validate the ModelState
            if (ModelState.IsValid)
            {
                // retrieve the member from the Repo
                Member m = unitOfWork.Members.GetMemberWithPosition(Convert.ToInt32(form.MemberId));
                // retrieve the DutyStatus
                MemberDutyStatus status = unitOfWork.MemberDutyStatus.Get(Convert.ToInt32(form.DutyStatus));
                // check if Member needs to be reassigned to the "Exception to Duty" Position in his ParentComponent
                if (status.DutyStatusName != "Full Duty")
                {
                    Position p = unitOfWork.Components.GetComponentWithPositions(Convert.ToInt32(form.ParentComponentId)).Positions.Where(x => x.Name == "Exception To Duty").FirstOrDefault();
                    if (p != null)
                    {                        
                        m.Position = p; 
                    }
                }
                // set the Member status to the new Status
                m.DutyStatus = status;
                // save changes to the repo
                unitOfWork.Complete();
                // return success object so Client can refresh the RosterManager
                return Json(new { success = true });
            }
            else
            {
                // Invalid ModelState, re-populate VM lists and return the ViewComponent
                form.StatusList = unitOfWork.MemberDutyStatus.GetMemberDutyStatusSelectListItems();
                form.Member = unitOfWork.Members.Get(Convert.ToInt32(form.MemberId));
                return ViewComponent("ChangeEmployeeStatus", form);
            }
            
        }

        // AssignMemberModalViewComponent
        [HttpGet]
        public IActionResult GetAssignMemberModalViewComponent(int positionId, int selectedComponentId)
        {
            Position p = unitOfWork.Positions.Get(positionId);
            List<MemberSelectListItem> members = unitOfWork.Members.GetAllMemberSelectListItems().ToList();
            AssignMemberModalViewComponentViewModel vm = new AssignMemberModalViewComponentViewModel(p, members, selectedComponentId);
            return ViewComponent("AssignMemberModal", vm);
        }

        public IActionResult GetAddEditComponentViewComponent(int componentId = 0, int parentComponentId = 0)
        {
            Component c = new Component();
            List<ComponentSelectListItem> components = new List<ComponentSelectListItem>();
            if (User.IsInRole("GlobalAdmin"))
            {
                components = unitOfWork.Components.GetComponentSelectListItems();
            }
            else if (User.IsInRole("ComponentAdmin"))
            {
                components = JsonConvert.DeserializeObject<List<ComponentSelectListItem>>(
                        User.Claims.FirstOrDefault(
                            claim => claim.Type == "CanEditComponents")
                            .Value
                            .ToString());
            }
            else
            {
                return Forbid();
            }

            if (componentId != 0)
            {
                c = unitOfWork.Components.Get(componentId);                                
            }
            if (parentComponentId != 0)
            {
                Component parent = unitOfWork.Components.Get(parentComponentId);
                c.ParentComponent = parent;
            }
            ComponentWithComponentListViewModel vm = new ComponentWithComponentListViewModel(c, components);
            return ViewComponent("ComponentAddEditModal", vm);
        }

        [HttpPost]
        public IActionResult UpdateComponent([Bind("ComponentId,ParentComponentId,ComponentName,LineupPosition,Acronym")] ComponentWithComponentListViewModel form)
        {
            if (ModelState.IsValid)
            {
                
                if (form.ComponentId != 0)
                {
                    if (unitOfWork.Components.SingleOrDefault(x => x.Name == form.ComponentName && x.ComponentId != form.ComponentId) != null)
                    {
                        ViewBag.Message = $"A Component with the name {form.ComponentName} already exists. Use a different Name.";
                        form.Components = unitOfWork.Components.GetComponentSelectListItems();
                        return ViewComponent("ComponentAddEditModal", form);
                    }
                    else
                    {
                        Component parentComponent = unitOfWork.Components.Get(Convert.ToInt32(form.ParentComponentId));
                        Component c = new Component()
                        {
                            ComponentId = Convert.ToInt32(form.ComponentId),
                            Acronym = form.Acronym,
                            Name = form.ComponentName,
                            LineupPosition = form.LineupPosition,
                            ParentComponent = parentComponent
                        };
                        unitOfWork.Components.UpdateComponentAndSetLineup(c);
                        unitOfWork.Complete();
                        return Json(new { Success = true });
                    }
                    
                }
                else
                {
                    Component parentComponent = unitOfWork.Components.Get(Convert.ToInt32(form.ParentComponentId));
                    Component c = new Component()
                    {                        
                        Acronym = form.Acronym,
                        Name = form.ComponentName,
                        LineupPosition = form.LineupPosition,
                        ParentComponent = parentComponent
                    };
                    unitOfWork.Components.UpdateComponentAndSetLineup(c);
                    unitOfWork.Complete();
                    return Json(new { Success = true });
                }

            }
            else
            {
                form.Components = unitOfWork.Components.GetComponentSelectListItems();
                return ViewComponent("ComponentAddEditModal", form);
            }
        }

        public IActionResult GetConfirmComponentDeleteModal(int componentId)
        {
            Component componentToDelete = unitOfWork.Components.GetComponentWithChildren(componentId);
            return ViewComponent("ConfirmComponentDeleteModal", componentToDelete);
        }

        public JsonResult DeleteComponent(int componentId)
        {
            unitOfWork.Components.RemoveComponent(componentId);
            unitOfWork.Complete();
            return Json(new { success = true });
        }

    }
}