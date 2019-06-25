using BlueDeck.Models.Enums;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BlueDeck.Controllers
{
    /// <summary>
    /// Controller that handles CRUD actions for the VehicleModels Enumeration
    /// </summary>
    /// <seealso cref="Controller" />
    public class VehicleModelsController : Controller
    {
        private IUnitOfWork unitOfWork;
        /// <summary>
        /// Property that sets the length of the index view pages
        /// </summary>
        public int PageSize = 25;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleModelsController"/> class.
        /// </summary>
        /// <param name="unit">An injected <see cref="IUnitOfWork"/> obtained from the services middleware.</param>
        public VehicleModelsController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// GET: VehicleModels/Index        
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleModels/Index")]
        public IActionResult Index(string sortOrder, string searchString, int page = 1)
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
            ViewBag.Title = "BlueDeck - Vehicle Models Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            vm.VehicleModels = vm.VehicleModels.Skip((page - 1) * PageSize).Take(PageSize);

            return View(vm);
        }


        /// <summary>
        /// GET: VehicleModels/Details/5        
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleModels/Details/{id:int}")]
        public IActionResult Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleModel = unitOfWork.VehicleModels.GetVehicleModelWithManufacturerAndVehicles((Int32)id);

            if (vehicleModel == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(vehicleModel);
        }

        /// <summary>
        /// GET: VehicleModels/Create        
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleModels/Create")]
        public IActionResult Create(string returnUrl)
        {
            AddEditVehicleModelViewModel vm = new AddEditVehicleModelViewModel(new VehicleModel(), unitOfWork.VehicleManufacturers.GetVehicleManufacturerSelectListItems());
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }


        /// <summary>
        ///  POST: VehicleModels/Create
        /// </summary>
        /// <param name="vehicleModel"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("VehicleModels/Create")]
        public IActionResult Create([Bind("VehicleModelName,ManufacturerId")] AddEditVehicleModelViewModel vehicleModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                VehicleModel toAdd = new VehicleModel()
                {
                    VehicleModelName = vehicleModel.VehicleModelName,
                    ManufacturerId = vehicleModel.ManufacturerId
                };
                unitOfWork.VehicleModels.Add(toAdd);
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
            ViewBag.ReturnUrl = returnUrl;
            vehicleModel.Manufacturers = unitOfWork.VehicleManufacturers.GetVehicleManufacturerSelectListItems();
            return View(vehicleModel);
        }

        /// <summary>
        /// GET: VehicleModels/Edit/5
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="returnUrl">An optional return Url </param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleModels/Edit/{id:int}")]
        public IActionResult Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleModel = unitOfWork.VehicleModels.Get((Int32)id);
            if (vehicleModel == null)
            {
                return NotFound();
            }
            AddEditVehicleModelViewModel vm = new AddEditVehicleModelViewModel(vehicleModel, unitOfWork.VehicleManufacturers.GetVehicleManufacturerSelectListItems());
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }

        /// <summary>
        /// POST: VehicleModels/Edit/5
        /// </summary>        
        /// <param name="vehicleModel">The vehicle manufacturer.</param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("VehicleModels/Edit/{id:int}")]
        public IActionResult Edit([Bind("VehicleModelId,VehicleModelName,ManufacturerId")] AddEditVehicleModelViewModel vehicleModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    VehicleModel toEdit = unitOfWork.VehicleModels.Get(vehicleModel.VehicleModelId);
                    if (toEdit != null)
                    {
                        toEdit.VehicleModelName = vehicleModel.VehicleModelName;
                        toEdit.ManufacturerId = vehicleModel.ManufacturerId;
                        unitOfWork.Complete();

                    }
                    else
                    {
                        return NotFound();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleModelExists(vehicleModel.VehicleModelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
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
            ViewBag.ReturnUrl = returnUrl;
            vehicleModel.Manufacturers = unitOfWork.VehicleManufacturers.GetVehicleManufacturerSelectListItems();
            return View(vehicleModel);
        }

        /// <summary>
        /// GET: VehicleModels/Delete/5
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="returnUrl">An optional return url for pass-through redirects.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleModels/Delete/{id:int}")]
        public IActionResult Delete(int? id, string returnUrl)
        {
            if (id == null || !VehicleModelExists((Int32)id))
            {
                return NotFound();
            }

            var vehicleModel = unitOfWork.VehicleModels.GetVehicleModelWithManufacturerAndVehicles((Int32)id);
            if (vehicleModel == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(vehicleModel);
        }

        /// <summary>
        /// POST: VehicleModels/Delete/5
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleModels/Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {

            var vehicleModel = unitOfWork.VehicleModels.Get(id);
            if (vehicleModel != null)
            {
                unitOfWork.VehicleModels.Remove(vehicleModel);
                unitOfWork.Complete();
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

        private bool VehicleModelExists(int id)
        {
            return (unitOfWork.VehicleModels.Find(e => e.VehicleModelId == id) != null);
        }
    }
}
