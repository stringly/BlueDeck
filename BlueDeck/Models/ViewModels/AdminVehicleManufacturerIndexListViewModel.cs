using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueDeck.Models.Enums;

namespace BlueDeck.Models.ViewModels
{
    /// <summary>
    /// Viewmodel for the Vehicle Entity used in the Admin/VehicleManufacturerIndex view.
    /// </summary>
    public class AdminVehicleManufacturerIndexListViewModel
    {
        /// <summary>
        /// Gets or sets the vehicles.
        /// </summary>
        /// <value>
        /// The vehicles.
        /// </value>
        public IEnumerable<VehicleManufacturer> VehicleManufacturers { get; set; }

        /// <summary>
        /// Gets or sets the paging information.
        /// </summary>
        /// <value>
        /// The paging information.
        /// </value>
        public PagingInfo PagingInfo { get; set; }

        /// <summary>
        /// Gets or sets the name sort.
        /// </summary>
        /// <value>
        /// The name sort.
        /// </value>
        public string NameSort { get; set; }

        /// <summary>
        /// Gets or sets the current filter.
        /// </summary>
        /// <value>
        /// The current filter.
        /// </value>
        public string CurrentFilter { get; set; }

        /// <summary>
        /// Gets or sets the current sort.
        /// </summary>
        /// <value>
        /// The current sort.
        /// </value>
        public string CurrentSort { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminVehicleManufacturerIndexListViewModel"/> class.
        /// </summary>
        public AdminVehicleManufacturerIndexListViewModel()
        {
            VehicleManufacturers = new List<VehicleManufacturer>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminVehicleManufacturerIndexListViewModel"/> class.
        /// </summary>
        /// <param name="_v">The vehicle manufacturers.</param>
        public AdminVehicleManufacturerIndexListViewModel(List<VehicleManufacturer> _v)
        {
            VehicleManufacturers = _v;
        }
    }
}
