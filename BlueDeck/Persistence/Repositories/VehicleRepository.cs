using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Types;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BlueDeck.Persistence.Repositories
{
    /// <summary>
    /// Repository for CRUD actions for the Vehicle Entity
    /// </summary>
    /// <seealso cref="Repository{Vehicle}" />
    /// <seealso cref="IVehicleRepository" />
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public VehicleRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the application database context.
        /// </summary>
        /// <value>
        /// The application database context.
        /// </value>
        public ApplicationDbContext ApplicationDbContext {
            get { return Context as ApplicationDbContext; }
        }

        /// <summary>
        /// Gets the vehicle select list items.
        /// </summary>
        /// <returns></returns>
        public List<VehicleSelectListItem> GetVehicleSelectListItems()
        {
            List<Vehicle> result = ApplicationDbContext.Vehicles
                                    .Include(x => x.Model)
                                    .ThenInclude(x => x.Manufacturer)
                                    .ToList();
                                        
            return result.ConvertAll(x => new VehicleSelectListItem(x));
        }

        /// <summary>
        /// Gets the vehicles with models.
        /// </summary>
        /// <returns></returns>
        public List<Vehicle> GetVehiclesWithModels()
        {
            return ApplicationDbContext.Vehicles
                        .Include(x => x.Model)
                        .ThenInclude(x => x.Manufacturer)
                        .ToList();
        }

        /// <summary>
        /// Gets the vehicle with model/manufacturer details.
        /// </summary>
        /// <param name="_vehicleId">The vehicle identifier.</param>
        /// <returns></returns>
        public Vehicle GetVehicleWithManufacturer(int _vehicleId)
        {
            return ApplicationDbContext.Vehicles
                .Include(x => x.Model)
                    .ThenInclude(x => x.Manufacturer)
                .Where(x => x.VehicleId == _vehicleId)
                .FirstOrDefault();
        }
    }

    
}
