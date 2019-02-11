using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using OrgChartDemo.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using OrgChartDemo.Models.Types;
using System;

namespace OrgChartDemo.Controllers
{
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
        public IActionResult Index()
        {
            var vm = new RosterManagerViewModel();
            vm.Components = unitOfWork.Components.GetComponentSelectListItems();
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
                    Dictionary<string, string> demoTableDictionary = new Dictionary<string, string>();     
                    List<Component> componentList = unitOfWork.Components.GetComponentAndChildren(selectedComponentId, new List<Component>());
                    RosterManagerViewComponentViewModel vm = new RosterManagerViewComponentViewModel(componentList);
                    demoTableDictionary = vm.GetDemoTableDictionaryForAllComponents();
                    return Json(new { Status = "Success", DemoDictionary = demoTableDictionary});
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
        
        // RosterManagerViewComponent
        /// <summary>
        /// Gets the RosterManager ViewComponent.
        /// </summary>
        /// <param name="componentId">The ComponentId of the top-level component</param>
        /// <returns></returns>
        public IActionResult GetRosterViewComponent(int componentId){
            List<Component> result = unitOfWork.Components.GetComponentAndChildren(componentId, new List<Component>());            
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

        // AddPositionToComponentModalViewComponent
        /// <summary>
        /// Gets the AddPositionToComponentViewComponent.
        /// </summary>
        /// <param name="componentId">The ComponentId of the Component to which a Position is being added</param>
        /// <returns>A <see cref="T:OrgChartDemo.ViewComponents.AddPositionToComponentViewComponent"/></returns>
        public IActionResult GetAddPositionToComponentViewComponent(int componentId)
        { 
            Component parent = unitOfWork.Components.Get(componentId);
            AddPositionToComponentViewComponentViewModel viewModel = new AddPositionToComponentViewComponentViewModel(parent);
            return ViewComponent("AddPositionToComponent", new { vm = viewModel });
        }

        /// <summary>
        /// Adds a new Position to a Component via POST from the AddPositionToComponentViewComponent.
        /// </summary>
        /// <param name="form">The POSTed <see cref="T:OrgChartDemo.Models.ViewModels.AddPositionToComponentViewComponentViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddPositionToComponent([Bind("ParentComponentId,PositionName,JobTitle,IsManager,IsUnique")] AddPositionToComponentViewComponentViewModel form)
        {
            // populate the Form object's ParentComponent property with it's ParentComponent
            form.ParentComponent = unitOfWork.Components.GetComponentWithChildren(Convert.ToInt32(form.ParentComponentId));
            // pull a list of all Positions in the Repo to use to check name conflict
            IEnumerable<Position> allPositions = unitOfWork.Positions.GetAll();

            // check Model validation first
            if (ModelState.IsValid)
            {
                // if ModelValidation passes, create a counter to store count of errors for other validation rules
                var errors = 0;                  
                // check to make sure submitted new position name doesn't conflict with an existing position
                foreach(Position p in allPositions)
                {
                    // check for conflict in Position Name
                    if (p.Name == form.PositionName)
                    {
                        errors++;
                        ViewBag.Message = $"A Position with the name {form.PositionName} already exists. Use a different Name.\n";                            
                    }                    
                }
                // check for conflict in "IsManager" for all Positions in the ParentComponent
                foreach(Position p in form.ParentComponent.Positions)
                {   
                    if (form.IsManager == true)
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
                    Position newPosition = new Position(){
                        ParentComponent = form.ParentComponent,
                        Name = form.PositionName,
                        JobTitle = form.JobTitle,
                        IsManager = form.IsManager,
                        IsUnique = form.IsUnique
                    };
                    unitOfWork.Positions.Add(newPosition);
                    unitOfWork.Complete();
                    // return a JSON object to the Ajax POST so that it can hide the Modal
                    return Json(new { Status = "Success" });
                }
                // validation errors, return the ViewModel with the ViewBag.Message showing
                else 
                {
                    return ViewComponent("AddPositionToComponent", form);
                }
            }
            else // ModelState invalid
            {   
                return ViewComponent("AddPositionToComponent", form);
            }
            
        }

        // EditEmployeeModalViewModalViewComponent
        public IActionResult GetEditEmployeeModalViewComponent(int memberId)
        {
            Member m = unitOfWork.Members.GetMemberWithDemographicsAndDutyStatus(memberId);            
            EditMemberModalViewComponentViewModel vm = new EditMemberModalViewComponentViewModel(m, unitOfWork.MemberRanks.GetMemberRankSelectListItems(),
                unitOfWork.MemberGenders.GetMemberGenderSelectListItems(),
                unitOfWork.MemberRaces.GetMemberRaceSelectListItems(),
                unitOfWork.MemberDutyStatus.GetMemberDutyStatusSelectListItems());
            return ViewComponent("EditMemberModal", vm);
                
        }

        [HttpPost]
        public IActionResult EditMemberModal([Bind("MemberId,MemberRank,MemberGender,MemberRace,FirstName,LastName,MiddleName,IdNumber,DutyStatusId,Email")] EditMemberModalViewComponentViewModel form)
        {
            Member m = unitOfWork.Members.Get(Convert.ToInt32(form.MemberId));
            if (ModelState.IsValid)
            {
                m.Rank = unitOfWork.MemberRanks.GetRankById(Convert.ToInt32(form.MemberRank));
                m.Gender = unitOfWork.MemberGenders.GetGenderById(Convert.ToInt32(form.MemberGender));
                m.Race = unitOfWork.MemberRaces.GetRaceById(Convert.ToInt32(form.MemberRace));
                m.FirstName = form.FirstName;
                m.MiddleName = form.MiddleName;
                m.LastName = form.LastName;
                m.IdNumber = form.IdNumber;
                m.DutyStatus = unitOfWork.MemberDutyStatus.GetStatusById(Convert.ToInt32(form.DutyStatus));
                m.Email = form.Email;
                unitOfWork.Complete();
                return Json(new { Status = "Success" });
            }
            else
            {
                EditMemberModalViewComponentViewModel vm = new EditMemberModalViewComponentViewModel(m, unitOfWork.MemberRanks.GetMemberRankSelectListItems(),
                unitOfWork.MemberGenders.GetMemberGenderSelectListItems(),
                unitOfWork.MemberRaces.GetMemberRaceSelectListItems(),
                unitOfWork.MemberDutyStatus.GetMemberDutyStatusSelectListItems());
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
                return Json(new { Status = "Success" });
            }
            else
            {
                // Invalid ModelState, re-populate VM lists and return the ViewComponent
                form.StatusList = unitOfWork.MemberDutyStatus.GetMemberDutyStatusSelectListItems();
                form.Member = unitOfWork.Members.Get(Convert.ToInt32(form.MemberId));
                return ViewComponent("ChangeEmployeeStatus", form);
            }
            
        }
    }
}