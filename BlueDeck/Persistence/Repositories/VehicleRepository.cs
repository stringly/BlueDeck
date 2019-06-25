using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Types;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System;

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
            List<Vehicle> result = new List<Vehicle>();
            result =  ApplicationDbContext.Vehicles
                        .Include(x => x.Model)
                        .ThenInclude(x => x.Manufacturer)
                        .Include(x => x.AssignedToComponent)
                        .Include(x => x.AssignedToPosition)
                        .Include(x => x.AssignedToMember)
                            .ThenInclude(x => x.Rank)
                        .ToList();
            return result;
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

        /// <summary>
        /// Gets the vehicles user can edit.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public List<VehicleSelectListItem> GetVehiclesUserCanEdit(int id)
        {
            List<VehicleSelectListItem> result = new List<VehicleSelectListItem>();
            SqlParameter param1 = new SqlParameter("@ComponentId", id);
            List<Component> components = ApplicationDbContext.Components.FromSql("dbo.GetComponentAndChildrenDemo @ComponentId", param1).ToList();
            List<int> componentIds = new List<int>();
            foreach(Component c in components)
            {
                componentIds.Add(c.ComponentId);
            }            
            List<Vehicle> componentVehicles = ApplicationDbContext.Vehicles.Where(x => componentIds.Contains((Int32)x.AssignedToComponentId))                
                    .Include(x => x.Model)
                        .ThenInclude(x => x.Manufacturer)
                .ToList();
                
            
          
            ApplicationDbContext.Set<Position>().Where(x => components.Contains(x.ParentComponent))
                .Include(x => x.AssignedVehicles)
                    .ThenInclude(x => x.Model)
                        .ThenInclude(x => x.Manufacturer)
                .Include(x => x.Members)
                    .ThenInclude(x => x.AssignedVehicle)
                        .ThenInclude(x => x.Model)
                            .ThenInclude(x => x.Manufacturer)
                .Include(x => x.TempMembers)
                    .ThenInclude(x => x.AssignedVehicle)
                        .ThenInclude(x => x.Model)
                            .ThenInclude(x => x.Manufacturer)
                .Load();

            // loop through the components
            foreach(Component c in components)
            {
                // first, add any vehicles assigned to the Component itself
                if (c.AssignedVehicles != null)
                {
                    foreach (Vehicle v in c.AssignedVehicles)
                    {
                        result.Add(new VehicleSelectListItem(v));
                    }
                }
                
                // next, loop through the Component's positions
                if(c.Positions != null)
                {
                    foreach (Position p in c.Positions)
                    {
                        if (p.AssignedVehicles != null)
                        {
                            foreach (Vehicle v in p.AssignedVehicles)
                            {
                                result.Add(new VehicleSelectListItem(v));
                            }
                        }
                        if (p.Members != null)
                        {
                            foreach (Member m in p.Members)
                            {
                                if (m.AssignedVehicle != null)
                                {
                                    result.Add(new VehicleSelectListItem(m.AssignedVehicle));
                                }                                
                            }
                        }
                        if (p.TempMembers != null)
                        {
                            // do the same for TDY Members
                            foreach (Member m in p.TempMembers)
                            {
                                if (m.AssignedVehicle != null)
                                {
                                    result.Add(new VehicleSelectListItem(m.AssignedVehicle));
                                }                                
                            }
                        }
                    }
                }
            }
            return result;
        }
    }

    
}
