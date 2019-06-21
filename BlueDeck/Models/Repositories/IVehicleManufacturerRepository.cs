using BlueDeck.Models.Types;
using System.Collections.Generic;
using BlueDeck.Models.Enums;

namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// Interface for a repository for the Vehicle Manufacturer Entity.
    /// </summary>
    /// <seealso cref="IRepository{VehicleManufacturer}" />
    public interface IVehicleManufacturerRepository : IRepository<VehicleManufacturer>
    {
        /// <summary>
        /// Gets or sets the get vehicle manufacturer select list items.
        /// </summary>
        /// <value>
        /// The get vehicle manufacturer select list items.
        /// </value>
        List<VehicleManufacturerSelectListItem> GetVehicleManufacturerSelectListItems();
    }
}
