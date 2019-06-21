using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.ViewModels
{
    /// <summary>
    /// Viewmodel for the Vehicle Entity used in the Admin/VehicleIndex view.
    /// </summary>
    public class AdminVehicleIndexListViewModel
    {
        /// <summary>
        /// Gets or sets the vehicles.
        /// </summary>
        /// <value>
        /// The vehicles.
        /// </value>
        public IEnumerable<Vehicle> Vehicles { get; set; }

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
        public string NumberSort { get; set; }

        /// <summary>
        /// Gets or sets the model name sort.
        /// </summary>
        /// <value>
        /// The model name sort.
        /// </value>
        public string ModelNameSort { get;set; }

        /// <summary>
        /// Gets or sets the manufacturer name sort.
        /// </summary>
        /// <value>
        /// The manufacturer name sort.
        /// </value>
        public string ManufacturerNameSort { get; set; }

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
        /// Initializes a new instance of the <see cref="AdminVehicleIndexListViewModel"/> class.
        /// </summary>
        public AdminVehicleIndexListViewModel()
        {
            Vehicles = new List<Vehicle>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminVehicleIndexListViewModel"/> class.
        /// </summary>
        /// <param name="_vehicles">The vehicles.</param>
        public AdminVehicleIndexListViewModel(List<Vehicle> _vehicles)
        {
            Vehicles = _vehicles;
        }
    }
}
