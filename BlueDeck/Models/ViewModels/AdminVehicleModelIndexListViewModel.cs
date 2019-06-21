using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueDeck.Models.Enums;

namespace BlueDeck.Models.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminVehicleModelIndexListViewModel
    {
                /// <summary>
        /// Gets or sets the vehicles.
        /// </summary>
        /// <value>
        /// The vehicles.
        /// </value>
        public IEnumerable<VehicleModel> VehicleModels { get; set; }

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
        /// Initializes a new instance of the <see cref="AdminVehicleModelIndexListViewModel"/> class.
        /// </summary>
        public AdminVehicleModelIndexListViewModel()
        {
            VehicleModels = new List<VehicleModel>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminVehicleModelIndexListViewModel"/> class.
        /// </summary>
        /// <param name="_v">The vehicles.</param>
        public AdminVehicleModelIndexListViewModel(List<VehicleModel> _v)
        {
            VehicleModels = _v;
        }
    }
}
