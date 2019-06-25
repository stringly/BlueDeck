using BlueDeck.Models.Enums;
using BlueDeck.Models.Types;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlueDeck.Models.ViewModels
{
    public class AddEditVehicleModelViewModel
    {
        /// <summary>
        /// Gets or sets the vehicle model identifier.
        /// </summary>
        /// <value>
        /// The vehicle model identifier.
        /// </value>
        [Display(Name = "VehicleModelId")]
        public int VehicleModelId { get; set; }

        /// <summary>
        /// Gets or sets the name of the vehicle model.
        /// </summary>
        /// <value>
        /// The name of the vehicle model.
        /// </value>
        [Display(Name = "Model Name")]
        [Required]
        public string VehicleModelName { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer identifier.
        /// </summary>
        /// <value>
        /// The manufacturer identifier.
        /// </value>
        [Display(Name = "Manufacturer")]
        [Required]
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturers.
        /// </summary>
        /// <value>
        /// The manufacturers.
        /// </value>
        public List<VehicleManufacturerSelectListItem> Manufacturers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEditVehicleModelViewModel"/> class.
        /// </summary>
        public AddEditVehicleModelViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEditVehicleModelViewModel"/> class.
        /// </summary>
        /// <param name="_m">The m.</param>
        /// <param name="_manufacturers">The manufacturers.</param>
        public AddEditVehicleModelViewModel(VehicleModel _m, List<VehicleManufacturerSelectListItem> _manufacturers)
        {
            VehicleModelId = _m.VehicleModelId;
            VehicleModelName = _m.VehicleModelName;
            ManufacturerId = _m.ManufacturerId;
            Manufacturers = _manufacturers;
        }


    }
}
