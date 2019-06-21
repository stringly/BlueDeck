using BlueDeck.Models.Enums;

namespace BlueDeck.Models.Types
{
    /// <summary>
    /// Type that represents vehicle manufacturers for use in Drop-down lists
    /// </summary>
    public class VehicleManufacturerSelectListItem
    {
        /// <summary>
        /// Gets or sets the vehicle manufacturer identifier.
        /// </summary>
        /// <value>
        /// The vehicle manufacturer identifier.
        /// </value>
        public int VehicleManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the vehicle manufacturer.
        /// </summary>
        /// <value>
        /// The name of the vehicle manufacturer.
        /// </value>
        public string VehicleManufacturerName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleManufacturerSelectListItem"/> class.
        /// </summary>
        public VehicleManufacturerSelectListItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleManufacturerSelectListItem"/> class.
        /// </summary>
        /// <param name="_v">The v.</param>
        public VehicleManufacturerSelectListItem(VehicleManufacturer _v)
        {
            VehicleManufacturerId = _v.VehicleManufacturerId;
            VehicleManufacturerName = _v.VehicleManufacturerName;
        }

    }
}
