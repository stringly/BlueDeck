using BlueDeck.Models.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.ViewModels
{
    public class AssignMemberModalViewComponentViewModel
    {
        /// <summary>
        /// Gets or sets the member identifier.
        /// </summary>
        /// <remarks>
        /// This is used to store the selection from the ViewComponent's Select list
        /// and POST to the handler. It will be null until a selection is made.
        /// </remarks>
        /// <value>
        /// The member identifier.
        /// </value>
        [Required]
        [Display(Name = "Available Members:")]
        public int MemberId { get; set; }
        /// <summary>
        /// Gets or sets the position identifier.
        /// </summary>
        /// <remarks>
        /// This is the PositionId of the Position wherein the user clicked the "assign a member" on the RosterManager
        /// It will be populated via the viewmodel's constructor when the RosterController invokes the ViewComponent
        /// </remarks>
        /// <value>
        /// The position identifier.
        /// </value>
        public int PositionId { get; set; }
        
        [Display(Name = "Search Member List:")]
        public string SearchString { get; set; }

        /// <summary>
        /// Gets or sets the selected component identifier.
        /// </summary>
        /// <remarks>
        /// This will be set in the Ajax POST for the AssignMember modal. It is used to store the current value of the 
        /// Component selection box on Roster/Index. We need this in case the Member assignment requires a full refresh 
        /// of the RosterManager ViewComponent as determined in RosterController/ReassignMember
        /// </remarks>
        /// <value>
        /// The selected component identifier.
        /// </value>
        public int SelectedComponentId { get; set; }

        /// <summary>
        /// Gets or sets the List of Members.
        /// </summary>
        /// <remarks>
        /// This list is used to populate a Select List from which a user can select a Member to assign.
        /// See <see cref="T:BlueDeck.Models.Types.MemberSelectListItem"/>
        /// </remarks>
        /// <value>
        /// The members.
        /// </value>
        public List<MemberSelectListItem> Members { get; set; }

        public Position Position { get; set; }

        public AssignMemberModalViewComponentViewModel()
        {
        }
        
        public AssignMemberModalViewComponentViewModel(Position p, List<MemberSelectListItem> members, int selectedComponentId)
        {
            PositionId = p.PositionId;
            Position = p;
            SelectedComponentId = selectedComponentId;
            Members = members;
        
        }
    }
}
