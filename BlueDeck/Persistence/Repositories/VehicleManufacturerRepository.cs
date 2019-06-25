using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Enums;
using BlueDeck.Models.Types;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BlueDeck.Persistence.Repositories
{
    /// <summary>
    /// Repository for CRUD actions for the Vehicle Manufacturer Enumeration.
    /// </summary>
    /// <seealso cref="Repository{VehicleManufacturer}" />
    /// <seealso cref="IVehicleManufacturerRepository" />
    public class VehicleManufacturerRepository : Repository<VehicleManufacturer>, IVehicleManufacturerRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleManufacturerRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public VehicleManufacturerRepository(ApplicationDbContext context) : base(context)
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
        /// Gets or sets the get vehicle manufacturer select list items.
        /// </summary>
        /// <returns></returns>
        /// <value>
        /// The get vehicle manufacturer select list items.
        /// </value>
        public List<VehicleManufacturerSelectListItem> GetVehicleManufacturerSelectListItems()
        {
            return GetAll().OrderBy(x => x.VehicleManufacturerName).ToList().ConvertAll(x => new VehicleManufacturerSelectListItem(x));
        }

        /// <summary>
        /// Gets the vehicle manufacturers with models.
        /// </summary>
        /// <returns></returns>
        public List<VehicleManufacturer> GetVehicleManufacturersWithModels()
        {
            return ApplicationDbContext.VehicleManufacturers.Include(x => x.Models).ToList();
        }

        /// <summary>
        /// Gets the vehicle manufacturer with models.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public VehicleManufacturer GetVehicleManufacturerWithModels(int id)
        {
            return ApplicationDbContext.VehicleManufacturers
                .Where(x => x.VehicleManufacturerId == id)
                .Include(x => x.Models)
                .FirstOrDefault();
        }
    }
}
