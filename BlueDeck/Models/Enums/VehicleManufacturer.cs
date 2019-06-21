using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlueDeck.Models.Enums
{
    /// <summary>
    /// Enumeration class for Vehicle Manufacturers
    /// </summary>
    public class VehicleManufacturer
    {
        /// <summary>
        /// Gets or sets the Vehicle manufacturer identifier.
        /// </summary>
        /// <value>
        /// The Vehicle manufacturer identifier.
        /// </value>
        [Key]
        [Display(Name = "Manufacturer Id")]
        public int VehicleManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the manufacturer.
        /// </summary>
        /// <value>
        /// The name of the manufacturer.
        /// </value>
        [Display(Name = "Manufacturer Name")]
        [Required]
        public string VehicleManufacturerName { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer's models.
        /// </summary>
        /// <value>
        /// The models.
        /// </value>
        public IEnumerable<VehicleModel> Models { get; set; }


    }
}
