using BlueDeck.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Display(Name = "VehicleId")]
        public int VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the model year.
        /// </summary>
        /// <value>
        /// The model year.
        /// </value>
        [Display(Name = "Model Year")]
        [Range(1990, 2050)]
        public int ModelYear { get; set; }

        /// <summary>
        /// Gets or sets the model identifier.
        /// </summary>
        /// <value>
        /// The model identifier.
        /// </value>
        [Display(Name = "Model")]
        public int ModelId { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>        
        public virtual VehicleModel Model { get; set; }

        /// <summary>
        /// Gets or sets the vin.
        /// </summary>
        /// <value>
        /// The vin.
        /// </value>
        [Display(Name = "VIN")]
        public string VIN { get; set; }

        /// <summary>
        /// Gets or sets the Vehicle's License plate number
        /// </summary>
        /// <value>
        /// The tag number.
        /// </value>
        [Display(Name = "Tag Number")]
        public string TagNumber { get; set; }


        /// <summary>
        /// Gets or sets the state of the tag.
        /// </summary>
        /// <value>
        /// The state of the tag.
        /// </value>
        [Display(Name = "State")]
        public string TagState { get; set; }

        /// <summary>
        /// Gets or sets the cruiser number.
        /// </summary>
        /// <value>
        /// The cruiser number.
        /// </value>
        [Display(Name = "Cruiser Number")]
        public string CruiserNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle is marked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this vehicle is marked; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Is Marked")]
        public bool IsMarked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has MVS.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has MVS; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "MVS Equipped")]
        public bool HasMVS { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has MDT.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has MDT; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "MDT Equipped")]
        public bool HasMDT { get; set; }

        /// <summary>
        /// Gets or sets the assigned to member identifier.
        /// </summary>
        /// <remarks>
        /// This is the MemberId of the Member to whom this vehicle is assigned. As of version 1.0, a vehicle can be assigned to one Member, Position, or Component at a time.
        /// </remarks>
        /// <value>
        /// The assigned to member identifier.
        /// </value>
        [Display(Name = "Assigned To Member")]
        [ForeignKey("AssignedToMember")]
        public int? AssignedToMemberId { get; set; }

        /// <summary>
        /// Gets or sets the assigned to member.
        /// </summary>
        /// <value>
        /// The assigned to member.
        /// </value>         
        public virtual Member AssignedToMember { get; set; }

        /// <summary>
        /// Gets or sets the assigned to position identifier.
        /// </summary>
        /// <remarks>
        /// This is the PositionId of the Member to whom this vehicle is assigned. As of version 1.0, a vehicle can be assigned to one Member, Position, or Component at a time.
        /// </remarks>
        /// <value>
        /// The assigned to position identifier.
        /// </value>
        [ForeignKey("AssignedToPosition")]
        public int? AssignedToPositionId { get; set; }

        /// <summary>
        /// Gets or sets the assigned to position.
        /// </summary>
        /// <value>
        /// The assigned to position.
        /// </value>      
        public virtual Position AssignedToPosition { get; set; }

        /// <summary>
        /// Gets or sets the assigned to component identifier.
        /// </summary>
        /// <value>
        /// <remarks>
        /// This is the MemberId of the Member to whom this vehicle is assigned. As of version 1.0, a vehicle can be assigned to one Member, Position, or Component at a time.
        /// </remarks>
        /// The assigned to component identifier.
        /// </value>
        [Display(Name ="Assigned To Component")]
        public int? AssignedToComponentId { get; set; }

        /// <summary>
        /// Gets or sets the assigned to component.
        /// </summary>
        /// <value>
        /// The assigned to component.
        /// </value>                
        public virtual Component AssignedToComponent { get; set; }

        /// <summary>
        /// Reassigns the vehicle to the member with the provided identifier.
        /// </summary>
        /// <remarks>
        /// Assigning the vehicle to a member will nullify any Position/Component assignment for the vehicle.
        /// </remarks>
        /// <param name="_memberId">The member identifier.</param>
        public void ReassignToMemberId(int _memberId)
        {
            AssignedToMemberId = Convert.ToInt32(_memberId);
            AssignedToPositionId = null;
            AssignedToComponentId = null;
        }

        /// <summary>
        /// Reassigns the vehicle to the position witht he provided identifier.
        /// </summary>
        /// <remarks>
        /// Assigning the vehicle to a Position will nullify any Member/Component assignment for the vehicle.
        /// </remarks>
        /// <param name="_positionId">The position identifier.</param>
        public void ReassignToPositionId(int _positionId)
        {
            AssignedToPositionId = Convert.ToInt32(_positionId);
            AssignedToMemberId = null;
            AssignedToComponentId = null;
        }

        /// <summary>
        /// Reassigns the vehicle to the component with the provided identifier.
        /// </summary>
        /// <remarks>
        /// Assigning the vehicle to a Componetn will nullify any Member/Position assignment for the vehicle.
        /// </remarks>
        /// <param name="_componentId">The component identifier.</param>
        public void ReassignToComponentId(int _componentId)
        {
            AssignedToComponentId = Convert.ToInt32(_componentId);
            AssignedToPositionId = null;
            AssignedToMemberId = null;
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <returns></returns>
        public string GetDisplayName()
        {
            if (Model != null)
            {
                if (Model.Manufacturer != null)
                {
                    return $"#{CruiserNumber} - {ModelYear} {Model.Manufacturer.VehicleManufacturerName} {Model.VehicleModelName} ({(IsMarked ? "Marked" : "Unmarked")})";
                }
                else
                {
                    return $"#{CruiserNumber} - {ModelYear} {Model.VehicleModelName} ({(IsMarked ? "Marked" : "Unmarked")})";
                }
            }
            else
            {
                return $"Vehicle #{CruiserNumber}";
            }
        }

        /// <summary>
        /// Returns the name of the Member, Position, or Component responsible for the vehicle.
        /// </summary>
        /// <returns></returns>
        public string IssuedTo()
        {
            if (AssignedToMember != null)
            {
                return AssignedToMember.GetTitleName();
            }
            else if (AssignedToPosition != null)
            {
                return $"{AssignedToPosition.Name} ({AssignedToPosition.ParentComponent.Name})";
            }
            else if (AssignedToComponent != null)
            {
                return AssignedToComponent.Name;
            }
            else
            {
                return "Unassigned";
            }
        }

    }
}
