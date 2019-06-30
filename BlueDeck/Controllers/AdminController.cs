using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlueDeck.Models;
using BlueDeck.Models.ViewModels;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Enums;

namespace BlueDeck.Controllers
{
    /// <summary>
    /// Controller that handles Administrative Functions
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize("IsGlobalAdmin")]
    [ApiExplorerSettings(IgnoreApi = true)]        
    public class AdminController : Controller
    {
        private IUnitOfWork unitOfWork;
        
        /// <summary>
        /// Property that controls the number of Items per page on the Index views
        /// </summary>
        public int PageSize = 25;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="unit">The Dependency-Injected <see cref="IUnitOfWork"/> obtained from the <see cref="Startup.ConfigureServices"/></param>
        public AdminController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// Indexes the specified return URL.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>The Admin/Index <see cref="ViewResult"/></returns>
        public IActionResult Index(string returnUrl)
        {
            AdminIndexViewModel vm = new AdminIndexViewModel();
            vm.GlobalAdmins = unitOfWork.Members.GetGlobalAdmins();
            vm.PendingAccounts = unitOfWork.Members.GetPendingAccounts();
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Title = "BlueDeck Admin - Main Page";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            return View(vm);
        }

        /// <summary>
        /// Returns the Admin/MemberIndex view
        /// </summary>
        /// <remarks>
        /// This view lists Members in the database with additional administrative fields. 
        /// </remarks>
        /// <param name="sortOrder">
        /// <para>
        /// Sort by: (ascend/descend)
        /// Last Name: [(nothing; LastName ascend is default/lastName_desc)]
        /// First Name: [(FirstName/firstName_desc)]
        /// Id Number: [(IDNumber/idNumber_desc)]
        /// Position Name: [(PositionName/positionName_desc)]
        /// By User Role: (filter: on/off) [(IsUserRoleOnly/IsUserRoleAny)]
        /// By Component Admin Role: (filter: on/off) [(IsComponentAdminOnly/IsComponentAdminAny)]
        /// By Global Admin Role: (filter: on/off) [(IsGlobalAdminOnly/IsGlobalAdminAny)]
        /// </para>
        /// </param>
        /// <param name="searchString">The search string.</param>
        /// <para>
        /// As of v1.0, the searchString parameter will search Members by Last Name, First Name, Position Name, and Id Number.
        /// The passed string will be converted to lowercase, and all non-alphanumeric characters will be removed.
        /// </para>
        /// <param name="page">
        /// <para>
        /// This controls pagination. The default value is 1.
        /// </para>
        /// </param>
        /// <returns>The Admin/MemberIndex <see cref="ViewResult"/></returns>
        public IActionResult MemberIndex(string sortOrder, string searchString, int page = 1)
        {
            AdminMemberIndexListViewModel vm = unitOfWork.Members.GetAdminMemberIndexListViewModel();
            vm.CurrentSort = sortOrder;
            vm.MemberLastNameSort = String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
            vm.MemberFirstNameSort = sortOrder == "FirstName" ? "firstName_desc" : "FirstName";
            vm.IdNumberSort = sortOrder == "IDNumber" ? "idNumber_desc" : "IDNumber";
            vm.PositionNameSort = sortOrder == "PositionName" ? "positionName_desc" : "PositionName";
            vm.IsUserRoleFilter = sortOrder == "IsUserRoleOnly" ? "IsUserRoleAny" : "IsUserRoleOnly";
            vm.IsComponentAdminRoleFilter = sortOrder == "IsComponentAdminOnly" ? "IsComponentAdminAny" : "IsComponentAdminOnly";
            vm.IsGlobalAdminRoleFilter = sortOrder == "IsGlobalAdminOnly" ? "IsGlobalAdminAny" : "IsGlobalAdminOnly";
            vm.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {                
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) 
                                  || char.IsWhiteSpace(c) 
                                  || c == '-')));
                string lowerString = new string(arr);
                lowerString = lowerString.ToLower();
                vm.Members = vm.Members
                    .Where(x => x.LastName.ToLower().Contains(lowerString) 
                    || x.FirstName.ToLower().Contains(lowerString) 
                    || x.PositionName.ToLower().Contains(lowerString)
                    || x.IdNumber.Contains(lowerString));
            }

            switch (sortOrder)
            {
                case "lastName_desc":
                    vm.Members = vm.Members.OrderByDescending(x => x.LastName);
                    break;
                case "firstName_desc":
                    vm.Members = vm.Members.OrderByDescending(x => x.FirstName);
                    break;
                case "FirstName":
                    vm.Members = vm.Members.OrderBy(x => x.FirstName);
                    break;
                case "idNumber_desc":
                    vm.Members = vm.Members.OrderByDescending(x => x.IdNumber);
                    break;
                case "IDNumber":
                    vm.Members = vm.Members.OrderBy(x => x.IdNumber);
                    break;
                case "PositionName":
                    vm.Members = vm.Members.OrderBy(x => x.PositionName);
                    break;
                case "positionName_desc":
                    vm.Members = vm.Members.OrderByDescending(x => x.PositionName);
                    break;
                case "IsUserRoleOnly":
                    vm.Members = vm.Members.Where(x => x.IsUser);
                    break;
                case "IsComponentAdminOnly":
                    vm.Members = vm.Members.Where(x => x.IsComponentAdmin);
                    break;
                case "IsGlobalAdminOnly":
                    vm.Members = vm.Members.Where(x => x.IsGlobalAdmin);
                    break;
                default:
                    vm.Members = vm.Members.OrderBy(x => x.AccountStateId).ThenBy(x => x.LastName);
                    break;
            }
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = searchString == null ? unitOfWork.Members.GetAll().Count() : vm.Members.Count()
            };
            ViewBag.Title = "BlueDeck Admin - Members Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            vm.Members = vm.Members.Skip((page - 1) * PageSize).Take(PageSize);

            return View(vm);            
        }

        /// <summary>
        /// Returns the Admin/PositionIndex view
        /// </summary>
        /// <remarks>
        /// This view lists Positions in the database with additional administrative fields. 
        /// </remarks>
        /// <param name="sortOrder">
        /// <para>
        /// Sort by: (ascend/descend)
        /// Position Name: [(nothing; PositionName ascend is default/name_desc)]
        /// Parent Component Name: [(ParentComponentName/parentName_desc)]        
        /// </para>
        /// </param>
        /// <param name="searchString">The search string.</param>
        /// <para>
        /// As of v1.0, the searchString parameter will search Positions by Position Name or Parent Component Name.
        /// The passed string will be converted to lowercase, and all non-alphanumeric characters will be removed.
        /// </para>
        /// <param name="page">
        /// <para>
        /// This controls pagination. The default value is 1.
        /// </para>
        /// </param>
        /// <returns>The Admin/PositionIndex <see cref="ViewResult"/></returns>
        public IActionResult PositionIndex(string sortOrder, string searchString, int page = 1)
        {
            AdminPositionIndexListViewModel vm = unitOfWork.Positions.GetAdminPositionIndexListViewModel();
            vm.CurrentSort = sortOrder;
            vm.PositionNameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            vm.ParentComponentNameSort = sortOrder == "ParentComponentName" ? "parentName_desc" : "ParentComponentName";
            vm.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) 
                                  || char.IsWhiteSpace(c) 
                                  || c == '-')));
                string lowerString = new string(arr);
                lowerString = lowerString.ToLower();
                vm.Positions = vm.Positions
                    .Where(x => x.PositionName.ToLower().Contains(lowerString) 
                    || x.ParentComponentName.ToLower().Contains(lowerString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    vm.Positions = vm.Positions.OrderByDescending(x => x.PositionName);
                    break;
                case "parentName_desc":
                    vm.Positions = vm.Positions.OrderByDescending(x => x.ParentComponentName);
                    break;
                case "ParentComponentName":
                    vm.Positions = vm.Positions.OrderBy(x => x.ParentComponentName).ThenBy(x => x.LineupPosition);
                    break;
            }
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = searchString == null ? unitOfWork.Positions.GetAll().Count() : vm.Positions.Count()
            };
            ViewBag.Title = "BlueDeck Admin - Positions Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            vm.Positions = vm.Positions.Skip((page - 1) * PageSize).Take(PageSize);

            return View(vm);  
        }

        /// <summary>
        /// Returns the Admin/ComponentIndex view
        /// </summary>
        /// <remarks>
        /// This view lists Components in the database with additional administrative fields. 
        /// </remarks>
        /// <param name="sortOrder">
        /// <para>
        /// Sort by: (ascend/descend)
        /// Component Name: [(nothing; Component Name ascend is default/name_desc)]
        /// Parent Component Name: [(ParentComponentName/parentName_desc)]        
        /// </para>
        /// </param>
        /// <param name="searchString">The search string.</param>
        /// <para>
        /// As of v1.0, the searchString parameter will search Components by Component Name or Parent Component Name.
        /// The passed string will be converted to lowercase, and all non-alphanumeric characters will be removed.
        /// </para>
        /// <param name="page">
        /// <para>
        /// This controls pagination. The default value is 1.
        /// </para>
        /// </param>         
        /// <returns>The Admin/ComponentIndex <see cref="ViewResult"/></returns>
        public IActionResult ComponentIndex(string sortOrder, string searchString, int page = 1)
        {
            AdminComponentIndexListViewModel vm = unitOfWork.Components.GetAdminComponentIndexListViewModel();
            vm.CurrentSort = sortOrder;
            vm.NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            vm.ParentComponentNameSort = sortOrder == "ParentComponentName" ? "parentName_desc" : "ParentComponentName";
            vm.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) 
                                  || char.IsWhiteSpace(c) 
                                  || c == '-')));
                string lowerString = new string(arr);
                lowerString = lowerString.ToLower();
                vm.Components = vm.Components
                    .Where(x => x.ComponentName.ToLower().Contains(lowerString) 
                    || x.ParentComponentName.ToLower().Contains(lowerString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    vm.Components = vm.Components.OrderByDescending(x => x.ComponentName);
                    break;
                case "parentName_desc":
                    vm.Components = vm.Components.OrderByDescending(x => x.ParentComponentName);
                    break;
                case "ParentComponentName":
                    vm.Components = vm.Components.OrderBy(x => x.ParentComponentName).ThenBy(x => x.LineupPosition);
                    break;
            }
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = searchString == null ? unitOfWork.Components.GetAll().Count() : vm.Components.Count()
            };
            ViewBag.Title = "BlueDeck Admin - Components Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            vm.Components = vm.Components.Skip((page - 1) * PageSize).Take(PageSize);

            return View(vm); 
        }

        /// <summary>
        /// Vehicles the index.
        /// </summary>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public IActionResult VehicleIndex(string sortOrder, string searchString, int page = 1)
        {
            AdminVehicleIndexListViewModel vm = new AdminVehicleIndexListViewModel(unitOfWork.Vehicles.GetVehiclesWithModels());
            vm.CurrentSort = sortOrder;
            vm.NumberSort = string.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            vm.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) 
                                  || char.IsWhiteSpace(c) 
                                  || c == '-')));
                string lowerString = new string(arr);
                lowerString = lowerString.ToLower();
                vm.Vehicles = vm.Vehicles
                    .Where(x => x.CruiserNumber.ToLower().Contains(lowerString));
            }

            switch (sortOrder)
            {
                case "number_desc":
                    vm.Vehicles = vm.Vehicles.OrderByDescending(x => x.CruiserNumber);
                    break;
            }
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = searchString == null ? unitOfWork.Vehicles.GetAll().Count() : vm.Vehicles.Count()
            };
            ViewBag.Title = "BlueDeck Admin - Vehicles Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            vm.Vehicles = vm.Vehicles.Skip((page - 1) * PageSize).Take(PageSize);

            return View(vm); 
        }

        /// <summary>
        /// Returns the VehicleModel Index list
        /// </summary>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public IActionResult VehicleModelIndex(string sortOrder, string searchString, int page = 1)
        {
            AdminVehicleModelIndexListViewModel vm = new AdminVehicleModelIndexListViewModel(unitOfWork.VehicleModels.GetVehicleModelsWithManufacturerAndVehicles());
            vm.CurrentSort = sortOrder;
            vm.NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            vm.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) 
                                  || char.IsWhiteSpace(c) 
                                  || c == '-')));
                string lowerString = new string(arr);
                lowerString = lowerString.ToLower();
                vm.VehicleModels = vm.VehicleModels
                    .Where(x => x.VehicleModelName.ToLower().Contains(lowerString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    vm.VehicleModels = vm.VehicleModels.OrderByDescending(x => x.VehicleModelName);
                    break;
            }
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = searchString == null ? unitOfWork.VehicleModels.GetAll().Count() : vm.VehicleModels.Count()
            };
            ViewBag.Title = "BlueDeck Admin - Vehicle Models Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            vm.VehicleModels = vm.VehicleModels.Skip((page - 1) * PageSize).Take(PageSize);

            return View(vm); 
        }

        /// <summary>
        /// Returns the Admin/VehicleManufacturerIndex view
        /// </summary>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public IActionResult VehicleManufacturerIndex(string sortOrder, string searchString, int page = 1)
        {
            AdminVehicleManufacturerIndexListViewModel vm = new AdminVehicleManufacturerIndexListViewModel(unitOfWork.VehicleManufacturers.GetVehicleManufacturersWithModels());
            vm.CurrentSort = sortOrder;
            vm.NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            vm.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c) 
                                  || char.IsWhiteSpace(c) 
                                  || c == '-')));
                string lowerString = new string(arr);
                lowerString = lowerString.ToLower();
                vm.VehicleManufacturers = vm.VehicleManufacturers
                    .Where(x => x.VehicleManufacturerName.ToLower().Contains(lowerString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    vm.VehicleManufacturers = vm.VehicleManufacturers.OrderByDescending(x => x.VehicleManufacturerName);
                    break;
            }
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = searchString == null ? unitOfWork.VehicleManufacturers.GetAll().Count() : vm.VehicleManufacturers.Count()
            };
            ViewBag.Title = "BlueDeck Admin - Vehicle Manufacturers Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            vm.VehicleManufacturers = vm.VehicleManufacturers.Skip((page - 1) * PageSize).Take(PageSize);

            return View(vm); 
        }

        /// <summary>
        /// Returns the Admin/AppStatusIndex view
        /// </summary>
        /// <remarks>
        /// This view lists App Statuses in the database with additional administrative fields. 
        /// </remarks>
        /// <returns>The Admin/AppStatusIndex <see cref="ViewResult"/></returns>
        public IActionResult AppStatusIndex()
        { 
            ViewBag.Title = "BlueDeck Admin - Application Status Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            ViewBag.Enumeration = true;
            return View(unitOfWork.AppStatuses.GetAppStatusesWithMemberCount());
        }

        /// <summary>
        /// Returns the Admin/DutyStatusIndex view
        /// </summary>
        /// <remarks>
        /// This view lists Duty Statuses in the database with additional administrative fields. 
        /// </remarks>
        /// <returns>The Admin/DutyStatusIndex <see cref="ViewResult"/></returns>
        public IActionResult DutyStatusIndex()
        {
            ViewBag.Title = "BlueDeck Admin - Duty Status Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            ViewBag.Enumeration = true;
            return View(unitOfWork.MemberDutyStatus.GetDutyStatusesWithMemberCount());
        }

        /// <summary>
        /// Returns the Admin/GenderIndex view
        /// </summary>
        /// <remarks>
        /// This view lists Genders in the database with additional administrative fields. 
        /// </remarks>
        /// <returns>The Admin/GenderIndex <see cref="ViewResult"/></returns>        
        public IActionResult GenderIndex()
        {
            ViewBag.Title = "BlueDeck Admin - Gender Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            ViewBag.Enumeration = true;
            return View(unitOfWork.MemberGenders.GetGendersWithMembers());
        }

        /// <summary>
        /// Returns the Admin/RaceIndex view
        /// </summary>
        /// <remarks>
        /// This view lists Races in the database with additional administrative fields. 
        /// </remarks>
        /// <returns>The Admin/RaceIndex <see cref="ViewResult"/></returns>    
        public IActionResult RaceIndex()
        {
            ViewBag.Title = "BlueDeck Admin - Race Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            ViewBag.Enumeration = true;
            return View(unitOfWork.MemberRaces.GetRacesWithMembers());
        }

        /// <summary>
        /// Returns the Admin/RankIndex view
        /// </summary>
        /// <remarks>
        /// This view lists Ranks in the database with additional administrative fields. 
        /// </remarks>
        /// <returns>The Admin/RankIndex <see cref="ViewResult"/></returns>  
        public IActionResult RankIndex()
        {
            ViewBag.Title = "BlueDeck Admin - Rank Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            ViewBag.Enumeration = true;
            return View(unitOfWork.MemberRanks.GetRanksWithMembers());
        }

        /// <summary>
        /// Returns the Admin/PhoneTypeIndex view
        /// </summary>
        /// <remarks>
        /// This view lists Phone Number Types in the database with additional administrative fields. 
        /// </remarks>
        /// <returns>The Admin/PhoneTypeIndex <see cref="ViewResult"/></returns>
        public IActionResult PhoneTypeIndex()
        {
            ViewBag.Title = "BlueDeck Admin - Phone Types Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            ViewBag.Enumeration = true;
            return View(unitOfWork.PhoneNumberTypes.GetPhoneNumberTypesWithPhoneNumbers());
        }

        /// <summary>
        /// Registers the user with the provided MemberId.
        /// </summary>
        /// <remarks>
        /// This method will change a Member account from "Pending" to "Active".
        /// It will also add the ComponentAdmin role if activating a Member assigned to a Manager/Assistant Manager Position.
        /// </remarks>
        /// <param name="id">The MemberId of the Member to activate.</param>
        /// <param name="returnUrl">The optional return URL.</param>
        /// <returns></returns>
        public IActionResult RegisterUser(int id, string returnUrl)
        {
            Member m = unitOfWork.Members.GetMemberWithPosition(id);
            if (m != null)
            {
                // retrieve the current user's Id to update the "Modified" fields on the Member being Registered
                var identity = (ClaimsIdentity)User.Identity;
                var claimMemberId = Convert.ToInt32(identity.Claims.FirstOrDefault(claim => claim.Type == "MemberId").Value.ToString());
                m.LastModified = DateTime.Now;
                m.LastModifiedById = claimMemberId;
                // set the Member's Account Status to Active
                AppStatus activeStatus = unitOfWork.AppStatuses.Find(x => x.StatusName == "Active").FirstOrDefault();
                if (activeStatus != null)
                {
                    m.AppStatus = activeStatus;
                }                
                // Add the "User" Role to the Member's Role Collection
                Role userRole = new Role();
                RoleType userRoleType = unitOfWork.RoleTypes.Find(x => x.RoleTypeName == "User").First();
                if (userRoleType != null)
                {
                    userRole.RoleType = userRoleType;
                    m.CurrentRoles.Add(userRole);
                }
                // check if the Member is in Manager/Assistant Manager Position, and add the ComponentAdmin role if so.
                // This should apply whether the Member is permanently assigned to the Position or is only TDY
                if (m?.Position?.IsManager == true || m?.Position?.IsAssistantManager == true || m?.TempPosition?.IsManager == true || m?.TempPosition?.IsAssistantManager == true)
                {
                    Role componentAdminRole = new Role();
                    RoleType componentAdminRoleType = unitOfWork.RoleTypes.Find(x => x.RoleTypeName == "ComponentAdmin").First();
                    if (componentAdminRoleType != null)
                    {
                        componentAdminRole.RoleType = componentAdminRoleType;
                        m.CurrentRoles.Add(componentAdminRole);
                    }
                }
                
                
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Member account successfully activated.";
            }
            else
            {
                TempData["Status"] = "Warning!";
                TempData["Message"] = "Member account could not be found.";
            }
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
        /// Changes a Member Account from "Pending" status back to "New" status.
        /// </summary>
        /// <param name="id">The MemberId of the Member.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        public IActionResult DenyUser(int id, string returnUrl)
        {
            Member m = unitOfWork.Members.Get(id);
            if (m != null)
            {
                var identity = (ClaimsIdentity)User.Identity;
                var claimMemberId = Convert.ToInt32(identity.Claims.FirstOrDefault(claim => claim.Type == "MemberId").Value.ToString());
                int newAccountStatusId = unitOfWork.AppStatuses.Find(x => x.StatusName == "New")?.FirstOrDefault()?.AppStatusId ?? 0;
                m.AppStatusId = newAccountStatusId;
                m.LastModified = DateTime.Now;
                m.LastModifiedById = claimMemberId;
                unitOfWork.Complete();
                TempData["Status"] = "Success!";
                TempData["Message"] = "Member account returned to 'New' status.";
            }
            else
            {
                TempData["Status"] = "Warning!";
                TempData["Message"] = "Member account could not be found.";
            }
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
}