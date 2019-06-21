using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueDeck.Models.Enums
{
    /// <summary>
    /// Enumeration Class for Vehicle Models
    /// </summary>
    public class VehicleModel
    {
        /// <summary>
        /// Gets or sets the Vehicle model identifier.
        /// </summary>
        /// <value>
        /// The Vehicle model identifier.
        /// </value>
        [Key]
        [Display(Name = "Auto Model Id")]
        public int VehicleModelId { get; set; }

        /// <summary>
        /// Gets or sets the name of the Vehicle model.
        /// </summary>
        /// <value>
        /// The name of the Vehicle model.
        /// </value>
        [Display(Name = "Model Name")]
        public string VehicleModelName { get; set; }

        /// <summary>
        /// Gets or sets the vehicle manufacturer identifier.
        /// </summary>
        /// <value>
        /// The vehicle manufacturer identifier.
        /// </value>

        public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        /// <value>
        /// The manufacturer.
        /// </value>        
        [ForeignKey("ManufacturerId")]
        public virtual VehicleManufacturer Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the vehicles.
        /// </summary>
        /// <value>
        /// The existing vehicles of this manufacturer.
        /// </value>
        public virtual List<Vehicle> Vehicles { get; set; }





    }
}
