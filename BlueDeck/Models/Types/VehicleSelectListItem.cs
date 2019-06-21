using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Types
{
    /// <summary>
    /// Type that contains basic information for a Vehicle for use in select lists
    /// </summary>
    public class VehicleSelectListItem
    {
        /// <summary>
        /// Gets or sets the vehicle identifier.
        /// </summary>
        /// <value>
        /// The vehicle identifier.
        /// </value>
        public int VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the vehicle.
        /// </summary>
        /// <remarks>
        /// As of version 1.0, this will be the Year/Make/Model/Marked of the vehicle
        /// </remarks>
        /// <value>
        /// The name of the vehicle.
        /// </value>
        public string VehicleName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleSelectListItem"/> class.
        /// </summary>
        public VehicleSelectListItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleSelectListItem"/> class.
        /// </summary>
        /// <param name="_v">The vehicle.</param>
        public VehicleSelectListItem(Vehicle _v)
        {
            VehicleId = _v.VehicleId;
            if (_v.Model != null)
            {
                VehicleName = $"#{_v.CruiserNumber} - {_v.ModelYear} {_v.Model.VehicleModelName} ({(_v.IsMarked ? "Marked" : "Unmarked")})";
            }
            else
            {
                VehicleName = $"#{_v.CruiserNumber} - ({(_v.IsMarked ? "Marked" : "Unmarked")})";
            }            
        }
    }
}
