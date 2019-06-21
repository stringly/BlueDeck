﻿using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Enums;
using BlueDeck.Models.Types;
using System.Collections.Generic;
using System.Linq;

namespace BlueDeck.Persistence.Repositories
{
    /// <summary>
    /// Repository for CRUD actions for the Vehicle Model Enumeration.
    /// </summary>
    /// <seealso cref="Repository{VehicleModel}" />
    /// <seealso cref="IVehicleModelRepository" />
    public class VehicleModelRepository : Repository<VehicleModel>, IVehicleModelRepository 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleModelRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public VehicleModelRepository(ApplicationDbContext context) : base(context)
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
        /// Gets or sets the get vehicle model select list items.
        /// </summary>
        /// <returns></returns>
        /// <value>
        /// The get vehicle model select list items.
        /// </value>
        public List<VehicleModelSelectListItem> GetVehicleModelSelectListItems()
        {
            return GetAll().ToList().ConvertAll(x => new VehicleModelSelectListItem(x));
        }
    }
}
