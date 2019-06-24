using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BlueDeck.Models.Types;

namespace BlueDeck.Models.ViewModels
{
    /// <summary>
    /// View Model used in Views that Create or Edit <see cref="Vehicle"/> objects.
    /// </summary>
    public class AddEditVehicleViewModel
    {
        /// <summary>
        /// Gets or sets the vehicle identifier (PK).
        /// </summary>
        /// <value>
        /// The vehicle identifier.
        /// </value>        
        [Display(Name = "VehicleId")]
        public int VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the model year.
        /// </summary>
        /// <value>
        /// The model year.
        /// </value>
        [Display(Name = "Model Year")]
        [Required]
        public int ModelYear { get; set; }

        /// <summary>
        /// Gets or sets the model identifier.
        /// </summary>
        /// <value>
        /// The model identifier.
        /// </value>
        [Display(Name = "Model")]
        [Required]
        public int ModelId { get; set; }

        /// <summary>
        /// Gets or sets the vin.
        /// </summary>
        /// <value>
        /// The vin.
        /// </value>
        [Display(Name = "VIN")]
        [Required]
        public string VIN { get; set; }

        /// <summary>
        /// Gets or sets the Vehicle's License plate number
        /// </summary>
        /// <value>
        /// The tag number.
        /// </value>
        [Display(Name = "Tag Number")]
        [Required]
        public string TagNumber { get; set; }


        /// <summary>
        /// Gets or sets the state of the tag.
        /// </summary>
        /// <value>
        /// The state of the tag.
        /// </value>
        [Display(Name = "State")]
        [Required]
        public string TagState { get; set; }

        /// <summary>
        /// Gets or sets the cruiser number.
        /// </summary>
        /// <value>
        /// The cruiser number.
        /// </value>
        [Display(Name = "Cruiser Number")]
        [Required]
        public string CruiserNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle is marked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this vehicle is marked; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Marked Vehicle")]
        public bool IsMarked {get;set;}

        /// <summary>
        /// Gets or sets the assigned to member identifier.
        /// </summary>
        /// <remarks>
        /// This is the MemberId of the Member to whom this vehicle is assigned. As of version 1.0, a vehicle can be assigned to one Member, Position, or Component at a time.
        /// </remarks>
        /// <value>
        /// The assigned to member identifier.
        /// </value>
        [Display(Name = "Assigned to Member")]
        public int? AssignedToMemberId { get; set; }

        /// <summary>
        /// Gets or sets the assigned to position identifier.
        /// </summary>
        /// <remarks>
        /// This is the PositionId of the Member to whom this vehicle is assigned. As of version 1.0, a vehicle can be assigned to one Member, Position, or Component at a time.
        /// </remarks>
        /// <value>
        /// The assigned to position identifier.
        /// </value>
        [Display(Name = "Assigned to Position")]
        public int? AssignedToPositionId { get; set; }

        /// <summary>
        /// Gets or sets the assigned to component identifier.
        /// </summary>
        /// <value>
        /// <remarks>
        /// This is the MemberId of the Member to whom this vehicle is assigned. As of version 1.0, a vehicle can be assigned to one Member, Position, or Component at a time.
        /// </remarks>
        /// The assigned to component identifier.
        /// </value>
        [Display(Name = "Assigned to Component")]
        public int? AssignedToComponentId { get; set; }

        /// <summary>
        /// Gets or sets the models.
        /// </summary>
        /// <value>
        /// The models.
        /// </value>
        public List<VehicleModelSelectListItem> Models { get;set; }

        /// <summary>
        /// Gets or sets the members list for use in select lists
        /// </summary>
        /// <value>
        /// The members.
        /// </value>
        public List<MemberSelectListItem> Members { get; set; }

        /// <summary>
        /// Gets or sets the positions for use in select lists.
        /// </summary>
        /// <value>
        /// The positions.
        /// </value>
        public List<PositionSelectListItem> Positions { get; set; }

        /// <summary>
        /// Gets or sets the components for use in select lists.
        /// </summary>
        /// <value>
        /// The components.
        /// </value>
        public List<ComponentSelectListItem> Components { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEditVehicleViewModel"/> class.
        /// </summary>
        public AddEditVehicleViewModel()
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEditVehicleViewModel"/> class.
        /// </summary>
        /// <param name="_v">A vehicle object.</param>
        public AddEditVehicleViewModel(Vehicle _v)
        {
            VehicleId = _v.VehicleId;
            ModelYear = _v.ModelYear;
            ModelId = _v.ModelId;
            VIN = _v.VIN;
            TagNumber = _v.TagNumber;
            TagState = _v.TagState;
            CruiserNumber = _v.CruiserNumber;
            IsMarked = _v.IsMarked;
            AssignedToMemberId = _v.AssignedToMemberId;
            AssignedToPositionId = _v.AssignedToPositionId;
            AssignedToComponentId = _v.AssignedToComponentId;
        }

    }
}
