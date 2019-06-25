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
    public class VehicleManufacturersController : Controller
    {
        private IUnitOfWork unitOfWork;
        /// <summary>
        /// Property that sets the length of the index view pages
        /// </summary>
        public int PageSize = 25;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleManufacturersController"/> class.
        /// </summary>
        /// <param name="unit">An injected <see cref="IUnitOfWork"/> obtained from the services middleware.</param>
        public VehicleManufacturersController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        /// <summary>
        /// GET: VehicleManufacturers/Index        
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleManufacturers/Index")]
        public IActionResult Index(string sortOrder, string searchString, int page = 1)
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
            ViewBag.Title = "BlueDeck Admin - Vehicles Index";
            ViewBag.Status = TempData["Status"]?.ToString() ?? "";
            ViewBag.Message = TempData["Message"]?.ToString() ?? "";
            vm.VehicleManufacturers = vm.VehicleManufacturers.Skip((page - 1) * PageSize).Take(PageSize);

            return View(vm);
        }


        /// <summary>
        /// GET: VehicleManufacturers/Details/5        
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleManufacturers/Details/{id:int}")]
        public IActionResult Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleManufacturer = unitOfWork.VehicleManufacturers.Get((Int32)id);

            if (vehicleManufacturer == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(vehicleManufacturer);
        }

        /// <summary>
        /// GET: VehicleManufacturers/Create        
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleManufacturers/Create")]
        public IActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        /// <summary>
        ///  POST: VehicleManufacturers/Create
        /// </summary>
        /// <param name="vehicleManufacturer">The vehicle manufacturer.</param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("VehicleManufacturers/Create")]
        public IActionResult Create([Bind("VehicleManufacturerName")] VehicleManufacturer vehicleManufacturer, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.VehicleManufacturers.Add(vehicleManufacturer);
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
            return View(vehicleManufacturer);
        }

        /// <summary>
        /// GET: VehicleManufacturers/Edit/5
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="returnUrl">An optional return Url </param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleManufacturers/Edit/{id:int}")]
        public IActionResult Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleManufacturer = unitOfWork.VehicleManufacturers.Get((Int32)id);
            if (vehicleManufacturer == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(vehicleManufacturer);
        }

        /// <summary>
        /// POST: VehicleManufacturers/Edit/5
        /// </summary>        
        /// <param name="vehicleManufacturer">The vehicle manufacturer.</param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("VehicleManufacturers/Edit/{id:int}")]
        public IActionResult Edit([Bind("VehicleManufacturerId,VehicleManufacturerName")] VehicleManufacturer vehicleManufacturer, string returnUrl)
        { 
            if (ModelState.IsValid)
            {
                try
                {
                    VehicleManufacturer toEdit = unitOfWork.VehicleManufacturers.Get(vehicleManufacturer.VehicleManufacturerId);
                    if (toEdit != null)
                    {
                        toEdit.VehicleManufacturerName = vehicleManufacturer.VehicleManufacturerName;
                        unitOfWork.Complete();

                    }
                    else {
                        return NotFound();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleManufacturerExists(vehicleManufacturer.VehicleManufacturerId))
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
            return View(vehicleManufacturer);
        }

        /// <summary>
        /// GET: VehicleManufacturers/Delete/5
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="returnUrl">An optional return url for pass-through redirects.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleManufacturers/Delete/{id:int}")]
        public IActionResult Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleManufacturer = unitOfWork.VehicleManufacturers.GetVehicleManufacturerWithModels((Int32)id);                
            if (vehicleManufacturer == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(vehicleManufacturer);
        }

        /// <summary>
        /// POST: VehicleManufacturers/Delete/5
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleManufacturers/Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {

            var vehicleManufacturer = unitOfWork.VehicleManufacturers.Get(id);
            if (vehicleManufacturer != null)
            {
                unitOfWork.VehicleManufacturers.Remove(vehicleManufacturer);
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

        private bool VehicleManufacturerExists(int id)
        {
            return (unitOfWork.VehicleManufacturers.Find(e => e.VehicleManufacturerId == id) != null);
        }
    }
}
