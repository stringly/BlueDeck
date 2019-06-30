using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models {
    /// <summary>
    /// Component Entity
    /// </summary>
    public class Component {
        /// <summary>
        /// Gets or sets the Component's Id (PK).
        /// </summary>
        /// <value>
        /// The Component's (PK) identifier.
        /// </value>
        [Key]
        public int ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the parent component identifier.
        /// </summary>
        /// <value>
        /// The parent component identifier.
        /// </value>
        public int? ParentComponentId {get;set;}

        /// <summary>
        /// Gets or sets the component's parent <see cref="Component"/>.
        /// </summary>
        /// <value>
        /// The Component's parent <see cref="Component"/>
        /// </value>
        [Display(Name = "Parent Component")]
        [ForeignKey("ParentComponentId")]
        public virtual Component ParentComponent { get; set; }

        /// <summary>
        /// Gets or sets the Component's name.
        /// </summary>
        /// <value>
        /// The name of the Component.
        /// </value>
        [Display(Name = "Component Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Component's acronym.
        /// </summary>
        /// <value>
        /// The acronym of the Component
        /// </value>        
        [Display(Name = "Acronym")]
        public string Acronym { get; set; }

        /// <summary>
        /// Gets or sets the lineup position.
        /// </summary>
        /// <remarks>
        /// This property is used to control the order in which the component is displayed among it's sibling components
        /// </remarks>
        /// <value>
        /// The lineup position.
        /// </value>
        public int? LineupPosition { get; set; }

                /// <summary>
        /// Gets or sets the assigned vehicles.
        /// </summary>
        /// <remarks>
        /// Represents a list of <see cref="Vehicle"/> that have been assigned to this Position.
        /// </remarks>
        /// <value>
        /// The assigned vehicles.
        /// </value>
        [Display(Name = "Assigned Vehicles")]
        public virtual List<Vehicle> AssignedVehicles { get; set; }

        /// <summary>
        /// Gets or sets the creator identifier.
        /// </summary>
        /// <value>
        /// The creator identifier.
        /// </value>
        public int? CreatorId { get; set; }

        /// <summary>
        /// Gets or sets the creator.
        /// </summary>
        /// <value>
        /// The creator.
        /// </value>
        [Display(Name = "Created By")]
        [ForeignKey("CreatorId")]
        public virtual Member Creator { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        /// <value>
        /// The last modified.
        /// </value>
        [Display(Name = "Date Last Modified")]
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Gets or sets the last modified by identifier.
        /// </summary>
        /// <value>
        /// The last modified by identifier.
        /// </value>
        public int? LastModifiedById { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>
        /// The last modified by.
        /// </value>
        [Display(Name = "Last Modified By")]
        [ForeignKey("LastModifiedById")]
        public virtual Member LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the list of the <see cref="Position"/>s assinged to this Component.
        /// </summary>
        /// <value>
        /// A list of <see cref="Position"/>s.
        /// </value>
        public virtual ICollection<Position> Positions { get; set; }

        /// <summary>
        /// Gets or sets the child components.
        /// </summary>
        /// <value>
        /// The child components.
        /// </value>
        public virtual ICollection<Component> ChildComponents { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Component"/> class.
        /// </summary>
        public Component()
        {            
        }

        /// <summary>
        /// Gets the display name of the manager.
        /// </summary>
        /// <returns></returns>
        public string GetManagerDisplayName()
        {
            return Positions?.Where(x => x.IsManager == true).FirstOrDefault()?.Members.First().GetTitleName() ?? "VACANT";
        }

        /// <summary>
        /// Gets the manager.
        /// </summary>
        /// <returns>A <see cref="Member"/> object that represents the Component's manageer, or null if no manager is found.</returns>
        public Member GetManager()
        {
            if (Positions != null)
            {
                Member manager = Positions?.Where(x => x.IsManager == true).FirstOrDefault()?.Members?.FirstOrDefault();
                if (manager == null)
                {
                    manager = Positions?.Where(x => x.IsManager == true).FirstOrDefault()?.TempMembers?.FirstOrDefault();
                }
                return manager;
            }
            else
            {
                return null;
            }
            
        }

        public Member GetAssistantManager()
        {
            if (Positions != null)
            {
                Member manager = Positions?.Where(x => x.IsAssistantManager == true).FirstOrDefault()?.Members?.FirstOrDefault();
                if(manager == null)
                {
                    manager = Positions?.Where(x => x.IsAssistantManager == true).FirstOrDefault()?.TempMembers?.FirstOrDefault();
                }
                return manager;
            }
            else
            {
                return null;
            }
            

        }
        /// <summary>
        /// Gets the worker count.
        /// </summary>
        /// <returns>An integer result of the count of all members assigned to non-managerial positions within the component and all of the component's children.</returns>
        public int GetWorkerCount()
        {
            int total = 0;
            if (ChildComponents != null && ChildComponents.Count > 0)
            {
                foreach (Component c in ChildComponents)
                {
                    total = total + c.GetWorkerCount();
                }
            }
            if (Positions != null && Positions.Count > 0)
            {
                foreach (Position p in Positions.Where(x => x.IsManager == false))
                {
                    total = total + p.Members.Count();
                }
            }
            
            return total;
        }

        /// <summary>
        /// Gets the manager count.
        /// </summary>
        /// <returns>An integer result of the count of all members assigned to Managerial positions within the component and all of it's children.</returns>
        public int GetManagerCount()
        {
            int total = 0;
            if (ChildComponents != null && ChildComponents.Count > 0)
            {
                foreach (Component c in ChildComponents)
                {
                    total = total + c.GetManagerCount();
                }
            }
            if (Positions != null && Positions.Count > 0)
            {
                foreach (Position p in Positions.Where(x => x.IsManager == true))
                {
                    total = total + p.Members.Count();
                }
            }            
            return total;
        }

        /// <summary>
        /// Returns a count of Members assigned to positions within the component that match the provided demographic parameters.
        /// </summary>
        /// <param name="genderId">The gender identifier.</param>
        /// <param name="raceId">The race identifier.</param>
        /// <param name="rankId">The rank identifier.</param>
        /// <returns>An integer count of matching members.</returns>
        public int MemberCount(int? genderId = null, int? raceId = null, int? rankId = null)
        {
            int total = 0;
            if (Positions == null || Positions.Count == 0)
            {
                return total;
            }
            else
            {
                foreach (Position p in Positions)
                {
                    
                    total += p?.Members
                        .Where(x => (genderId == null || x.GenderId == genderId)
                        && (raceId == null || x.RaceId == raceId)
                        && (rankId == null || x.RankId == rankId))
                    .Count() ?? 0;
                }
                return total;
            }
        }

        /// <summary>
        /// Counts the number of Members assigned to Positions within the Component and all of it's children that match the provided demographic parameters.
        /// </summary>
        /// <param name="genderId">The gender identifier.</param>
        /// <param name="raceId">The race identifier.</param>
        /// <param name="rankId">The rank identifier.</param>
        /// <returns>An integer count of matching members.</returns>
        public int MemberCountRecursive(int? genderId = null, int? raceId = null, int? rankId = null)
        {
            int total = 0;
            if (ChildComponents != null && ChildComponents.Count > 0)
            {
                foreach (Component c in ChildComponents)
                {
                    total += c.MemberCountRecursive(genderId, raceId, rankId);
                }
            }
            if (Positions == null || Positions.Count == 0)
            {
                return total;
            }
            else
            {
                foreach (Position p in Positions)
                {
                    
                    total += p?.Members
                        .Where(x => (genderId == null || x.GenderId == genderId)
                        && (raceId == null || x.RaceId == raceId)
                        && (rankId == null || x.RankId == rankId))
                    .Count() ?? 0;
                }
                return total;
            }
        }

        /// <summary>
        /// Gets the component members.
        /// </summary>
        /// <returns>A list of <see cref="Member"/> objects of the members assigned to Positions within this component.</returns>
        public List<Member> GetComponentMembers()
        {
            List<Member> result = new List<Member>();
            if (Positions != null && Positions.Count > 0)
            {
                foreach (Position p in Positions)
                {
                    result.AddRange(p.Members);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the members assigned to Positions in the Component and the Component's children.
        /// </summary>
        /// <returns>A list of <see cref="Member"/> objects of the Members assigned to Positions within this component and all of it's children.</returns>
        public List<Member> GetComponentMembersRecursive()
        {
            List<Member> result = new List<Member>();
            if(ChildComponents != null && ChildComponents.Count > 0)
            {
                foreach(Component child in ChildComponents)
                {
                    result.AddRange(child.GetComponentMembersRecursive());
                }
            }
            if (Positions != null && Positions.Count > 0)
            {
                foreach(Position p in Positions)
                {
                    result.AddRange(p.Members);
                }
            }            
            return result;
        }

        /// <summary>
        /// Gets the exception to duty members.
        /// </summary>
        /// <returns>A list of <see cref="Member"/> objects assigned to Positions within the Component that are in a <see cref="Enums.DutyStatus"/> that is an "Exception to Normal Duty."</returns>
        public List<Member> GetExceptionToDutyMembers()
        {
            List<Member> result = new List<Member>();
            if (Positions != null && Positions.Count > 0)
            {
                foreach (Position p in Positions)
                {
                    result.AddRange(p.Members.Where(x => x.DutyStatus.IsExceptionToNormalDuty == true || x.TempPositionId != null).ToList());
                }
            }            
            return result;
        }

        /// <summary>
        /// Gets the exception to duty members from the component and all of it's children.
        /// </summary>
        /// <returns>A list of <see cref="Member"/> objects assigned to Positions within the Component and all of it's children that are in a <see cref="Enums.DutyStatus"/> that is an "Exception to Normal Duty."</returns>
        public List<Member> GetExceptionToDutyMembersRecursive()
        {
            List<Member> result = new List<Member>();
            if(ChildComponents != null && ChildComponents.Count > 0)
            {
                foreach(Component child in ChildComponents)
                {
                    result.AddRange(child.GetExceptionToDutyMembersRecursive());
                }
            }
            if (Positions != null && Positions.Count > 0)
            {
                foreach(Position p in Positions)
                {
                    result.AddRange(p.Members.Where(x => x.DutyStatus.IsExceptionToNormalDuty == true || x.TempPositionId != null).ToList());
                }
            }            
            return result;
        }
    }
}
