using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BlueDeck.Models.Types;

namespace BlueDeck.Models
{
    /// <summary>
    /// Vehicle Entity
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// Gets or sets the vehicle identifier (PK).
        /// </summary>
        /// <value>
        /// The vehicle identifier.
        /// </value>
        [Key]
        public int VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the model year.
        /// </summary>
        /// <value>
        /// The model year.
        /// </value>
        public int ModelYear { get; set; }

        

    }
}
