using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Enums;
using Microsoft.AspNetCore.Http;
using BlueDeck.Models.ViewModels;

namespace BlueDeck.Controllers
{
    /// <summary>
    /// Controller that handles CRUD actions for the <see cref="Vehicle"/> entity.
    /// </summary>
    /// <seealso cref="Controller" />
    public class VehicleController : Controller
    {
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Property that sets the length of the index view pages
        /// </summary>
        public int PageSize = 25;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleController"/> class.
        /// </summary>
        /// <param name="unit">The injected <see cref="IUnitOfWork"/> obtained from the services middleware.</param>
        public VehicleController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// GET: Vehicles/Index
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Vehicles/Index")]
        public IActionResult Index(string sortOrder, string searchString, int page = 1)
        {
            VehicleIndexListViewModel vm = new VehicleIndexListViewModel(unitOfWork.Vehicles.GetVehiclesWithModels().ToList());
            vm.CurrentSort = sortOrder;
            vm.ModelNameSort = string.IsNullOrEmpty(sortOrder) ? "modelName_desc" : "";
            vm.ManufacturerNameSort = sortOrder == "ManufacturerName" ? "manufacturerName_desc" : "ManufacturerName";
            vm.ModelYearSort = sortOrder == "ModelYear" ? "modelYear_desc" : "ModelYear";
            vm.CruiserNumberSort = sortOrder == "CruiserNumber" ? "cruiserNumber_desc" : "CruiserNumber";
            vm.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                vm.Vehicles = vm.Vehicles
                    .Where(x => x.CruiserNumber.Contains(searchString) || x.Model.VehicleModelName.Contains(searchString) || x.Model.Manufacturer.VehicleManufacturerName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "modelName_desc":
                    vm.Vehicles = vm.Vehicles.OrderByDescending(x => x.Model.VehicleModelName);
                    break;
                case "ManufacturerName":
                    vm.Vehicles = vm.Vehicles.OrderBy(x => x.Model.Manufacturer.VehicleManufacturerName);
                    break;
                case "manufacturerName_desc":
                    vm.Vehicles = vm.Vehicles.OrderByDescending(x => x.Model.Manufacturer.VehicleManufacturerName);
                    break;
                case "ModelYear":
                    vm.Vehicles = vm.Vehicles.OrderBy(x => x.ModelYear);
                    break;
                case "modelYear_desc":
                    vm.Vehicles = vm.Vehicles.OrderByDescending(x => x.ModelYear);
                    break;
                case "CruiserNumber":
                    vm.Vehicles = vm.Vehicles.OrderBy(x => x.CruiserNumber);
                    break;
                case "cruiserNumber_desc":
                    vm.Vehicles = vm.Vehicles.OrderByDescending(x => x.CruiserNumber);
                    break;
                default:
                    vm.Vehicles = vm.Vehicles.OrderBy(x => x.CruiserNumber);
                    break;
            }
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = searchString == null ? unitOfWork.Positions.GetAll().Count() : vm.Vehicles.Count()
            };
            ViewBag.Title = "BlueDeck Vehicle Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            vm.Vehicles = vm.Vehicles.Skip((page - 1) * PageSize).Take(PageSize);
            return View(vm);
        }

        /// <summary>
        /// Detailses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Vehicles/Details/{id:int}")]
        public ActionResult Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vehicle = unitOfWork.Vehicles.GetVehicleWithManufacturer((Int32)id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(vehicle);
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Vehicles/Create")]
        public ActionResult Create(string returnUrl)
        {
            
            AddEditVehicleViewModel vm = new AddEditVehicleViewModel(new Vehicle());
            // TODO: Limit Member/Position/Component select lists based on user role
            vm.Models = unitOfWork.VehicleModels.GetVehicleModelSelectListItems();
            vm.Members = unitOfWork.Members.GetAllMemberSelectListItems().ToList();
            vm.Positions = unitOfWork.Positions.GetAllPositionSelectListItems().ToList();
            vm.Components = unitOfWork.Components.GetComponentSelectListItems().ToList();
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Title = "Create New Position";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }

        /// <summary>
        /// Creates the specified form.
        /// </summary>
        /// <param name="form">The form data, bound to a <see cref="AddEditVehicleViewModel"/></param>
        /// <param name="returnUrl">An optional returnUrl used for redirection.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Vehicles/Create")]
        public ActionResult Create([Bind("ModelYear,ModelId,VIN,TagNumber,TagState,CruiserNumber,IsMarked,AssignedToMemberId,AssignedToPositionId,AssignedToComponentId")] AddEditVehicleViewModel form, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Create Vehicle: Corrections Required";
                ViewBag.Status = "Warning!";
                ViewBag.Message = "You must correct the fields indicated.";
                //TODO: Limit select lists based on user role
                form.Models = unitOfWork.VehicleModels.GetVehicleModelSelectListItems();
                form.Members = unitOfWork.Members.GetAllMemberSelectListItems().ToList();
                form.Positions = unitOfWork.Positions.GetAllPositionSelectListItems().ToList();
                form.Components = unitOfWork.Components.GetComponentSelectListItems().ToList();
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            int errors = 0;
            // ensure that the cruiser number is not in use
            if(unitOfWork.Vehicles.SingleOrDefault(x => x.CruiserNumber == form.CruiserNumber) != null)
            {
                errors++;
                ViewBag.Message = $"The cruiser number {form.CruiserNumber} is in use. Choose another.";
            }
            // ensure that the Tag Number is not in use
            if(unitOfWork.Vehicles.SingleOrDefault(x => x.TagNumber == form.TagNumber && x.TagState == form.TagState) != null)
            {
                errors++;
                ViewBag.Message = $"The Tag Number {form.TagNumber}/{form.TagState} is in use. Choose another.";                
            }

            if(errors == 0)
            {
                Vehicle v = new Vehicle()
                {
                    ModelYear = form.ModelYear,
                    ModelId = form.ModelId,
                    VIN = form.VIN,
                    TagNumber = form.TagNumber,
                    TagState = form.TagState,
                    CruiserNumber = form.CruiserNumber,
                    IsMarked = form.IsMarked,
                    AssignedToMemberId = form.AssignedToMemberId,
                    AssignedToPositionId = form.AssignedToPositionId,
                    AssignedToComponentId = form.AssignedToComponentId
                };
                unitOfWork.Vehicles.Add(v);
                unitOfWork.Complete();
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                // TODO: limit select lists based on user role
                ViewBag.Title = "Create Vehicle: Corrections Required";
                ViewBag.Status = "Warning!";
                //TODO: Limit select lists based on user role
                form.Models = unitOfWork.VehicleModels.GetVehicleModelSelectListItems();
                form.Members = unitOfWork.Members.GetAllMemberSelectListItems().ToList();
                form.Positions = unitOfWork.Positions.GetAllPositionSelectListItems().ToList();
                form.Components = unitOfWork.Components.GetComponentSelectListItems().ToList();
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }         

        }

        /// <summary>
        /// GET: Vehicles/Edit/4
        /// </summary>
        /// <remarks>
        /// Returns a view that allows a user to edit an existing vehicle.
        /// </remarks>
        /// <param name="id">The identifier.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Vehicles/Edit/{id:int}")]
        public ActionResult Edit(int id, string returnUrl)
        {
            if (!VehicleExists(id))
            {
                return NotFound();
            }
            Vehicle toEdit = unitOfWork.Vehicles.Get(id);
            
            AddEditVehicleViewModel vm = new AddEditVehicleViewModel(toEdit);
            // TODO: Limit Member/Position/Component select lists based on user role
            vm.Models = unitOfWork.VehicleModels.GetVehicleModelSelectListItems();
            vm.Members = unitOfWork.Members.GetAllMemberSelectListItems().ToList();
            vm.Positions = unitOfWork.Positions.GetAllPositionSelectListItems().ToList();
            vm.Components = unitOfWork.Components.GetComponentSelectListItems().ToList();
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Title = "Create New Position";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);            
        }

        /// <summary>
        /// POST: Vehicle/Edit/5
        /// </summary>
        /// <remarks>
        /// Creates a new vehicle based on a POSTed form object, bound to a <see cref="AddEditVehicleViewModel"/>
        /// </remarks>
        /// <param name="form">The form data, bound to a <see cref="AddEditVehicleViewModel"/>.</param>
        /// <param name="returnUrl">An optional return URL used for pass-through redirects.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Vehicles/Edit/{id:int}")]
        public ActionResult Edit([Bind("ModelYear,ModelId,VIN,TagNumber,TagState,CruiserNumber,IsMarked,AssignedToMemberId,AssignedToPositionId,AssignedToComponentId")] AddEditVehicleViewModel form, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Create Vehicle: Corrections Required";
                ViewBag.Status = "Warning!";
                ViewBag.Message = "You must correct the fields indicated.";
                //TODO: Limit select lists based on user role
                form.Models = unitOfWork.VehicleModels.GetVehicleModelSelectListItems();
                form.Members = unitOfWork.Members.GetAllMemberSelectListItems().ToList();
                form.Positions = unitOfWork.Positions.GetAllPositionSelectListItems().ToList();
                form.Components = unitOfWork.Components.GetComponentSelectListItems().ToList();
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            int errors = 0;
            // ensure that the cruiser number is not in use
            if(unitOfWork.Vehicles.SingleOrDefault(x => x.CruiserNumber == form.CruiserNumber && x.VehicleId != form.VehicleId) != null)
            {
                errors++;
                ViewBag.Message = $"The cruiser number {form.CruiserNumber} is in use. Choose another.";
            }
            // ensure that the Tag Number is not in use
            if(unitOfWork.Vehicles.SingleOrDefault(x => x.TagNumber == form.TagNumber && x.TagState == form.TagState && x.VehicleId != form.VehicleId) != null)
            {
                errors++;
                ViewBag.Message = $"The Tag Number {form.TagNumber}/{form.TagState} is in use. Choose another.";                
            }

            if(errors == 0)
            {
                Vehicle toEdit = unitOfWork.Vehicles.Get(form.VehicleId);
                toEdit.ModelYear = form.ModelYear;
                toEdit.ModelId = form.ModelId;
                toEdit.VIN = form.VIN;
                toEdit.TagNumber = form.TagNumber;
                toEdit.TagState = form.TagState;
                toEdit.CruiserNumber = form.CruiserNumber;
                toEdit.IsMarked = form.IsMarked;
                toEdit.AssignedToMemberId = form.AssignedToMemberId;
                toEdit.AssignedToPositionId = form.AssignedToPositionId;
                toEdit.AssignedToComponentId = form.AssignedToComponentId;
                unitOfWork.Complete();

                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                // TODO: limit select lists based on user role
                ViewBag.Title = "Create Vehicle: Corrections Required";
                ViewBag.Status = "Warning!";
                //TODO: Limit select lists based on user role
                form.Models = unitOfWork.VehicleModels.GetVehicleModelSelectListItems();
                form.Members = unitOfWork.Members.GetAllMemberSelectListItems().ToList();
                form.Positions = unitOfWork.Positions.GetAllPositionSelectListItems().ToList();
                form.Components = unitOfWork.Components.GetComponentSelectListItems().ToList();
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }  
        }

        /// <summary>
        /// GET: Vehicles/Delete/5
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="returnUrl">An optional return URL used for pass-through redirects.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Vehicles/Delete/{id:int}")]
        public ActionResult Delete(int id, string returnUrl)
        {
            if (!VehicleExists(id))
            {
                return NotFound();
            }
            var vehicle = unitOfWork.Vehicles.Get(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Confirm - Delete Vehicle?";
            ViewBag.ReturnUrl = returnUrl;
            return View(vehicle);
        }

        /// <summary>
        /// POST: Vehicle/Delete/5
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="returnUrl">An optional returnUrl used for pass-through redirects.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Vehicles/Delete/{id:int}")]
        public ActionResult DeleteConfirmed(int id, string returnUrl)
        {
            if (!VehicleExists(id))
            {
                return NotFound();
            }
            else
            {
                Vehicle toRemove = unitOfWork.Vehicles.Get(id);
                unitOfWork.Vehicles.Remove(toRemove);
                unitOfWork.Complete();
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

        private bool VehicleExists(int id)
        {
            return (unitOfWork.Vehicles.Find(e => e.VehicleId == id) != null);
        }
    }
}