using BlueDeck.Models.Types;
using System.Collections.Generic;

namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// Interface for a repository for the Vehicle Entity.
    /// </summary>
    /// <seealso cref="IRepository{Vehicle}" />
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        /// <summary>
        /// Gets or sets the get vehicle select list items.
        /// </summary>
        /// <value>
        /// The get vehicle select list items.
        /// </value>
        List<VehicleSelectListItem> GetVehicleSelectListItems();

        /// <summary>
        /// Gets the vehicles with models.
        /// </summary>
        /// <returns></returns>
        List<Vehicle> GetVehiclesWithModels();

        /// <summary>
        /// Gets the vehicle with model/manufacturer details.
        /// </summary>
        /// <param name="_vehicleId">The vehicle identifier.</param>
        /// <returns></returns>
        Vehicle GetVehicleWithManufacturer(int _vehicleId);

        /// <summary>
        /// Gets the vehicles user can edit.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        List<VehicleSelectListItem> GetVehiclesUserCanEdit(int id);


    }
}
