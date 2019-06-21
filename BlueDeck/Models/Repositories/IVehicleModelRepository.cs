using BlueDeck.Models.Types;
using System.Collections.Generic;
using BlueDeck.Models.Enums;

namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// Interface for a repository for the Vehicle Model Entity.
    /// </summary>
    /// <seealso cref="IRepository{VehicleModel}" />
    public interface IVehicleModelRepository : IRepository<VehicleModel>
    {
        /// <summary>
        /// Gets or sets the get vehicle model select list items.
        /// </summary>
        /// <value>
        /// The get vehicle model select list items.
        /// </value>
        List<VehicleModelSelectListItem> GetVehicleModelSelectListItems();
    }
}
