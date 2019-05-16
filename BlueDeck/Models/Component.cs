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

        public int? ParentComponentId {get;set;}

        /// <summary>
        /// Gets or sets the component's parent <see cref="T:BlueDeck.Models.Component"/>.
        /// </summary>
        /// <value>
        /// The Component's parent <see cref="T:BlueDeck.Models.Component"/>
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
        public int? CreatorId { get; set; }
        [Display(Name = "Created By")]
        [ForeignKey("CreatorId")]
        public virtual Member Creator { get; set; }
        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Date Last Modified")]
        public DateTime LastModified { get; set; }
        
        public int? LastModifiedById { get; set; }
        [Display(Name = "Last Modified By")]
        [ForeignKey("LastModifiedById")]
        public virtual Member LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the list of the <see cref="T:BlueDeck.Models.Position"/>s assinged to this Component.
        /// </summary>
        /// <value>
        /// An <see cref="T:ICollection{T}"/> of <see cref="T:BlueDeck.Models.Position"/>s.
        /// </value>
        public virtual ICollection<Position> Positions { get; set; }
        
        public virtual ICollection<Component> ChildComponents { get; set; }

        public Component()
        {            
        }
        public string GetManagerDisplayName()
        {
            return Positions?.Where(x => x.IsManager == true).FirstOrDefault()?.Members.First().GetTitleName() ?? "VACANT";
        }
        public Member GetManager()
        {
            return Positions?.Where(x => x.IsManager == true).FirstOrDefault().Members.FirstOrDefault() ?? null;
        }        
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

        public List<Member> GetExceptionToDutyMembers()
        {
            List<Member> result = new List<Member>();
            if (Positions != null && Positions.Count > 0)
            {
                foreach (Position p in Positions)
                {
                    result.AddRange(p.Members.Where(x => x.DutyStatus.IsExceptionToNormalDuty == true).ToList());
                }
            }            
            return result;
        }

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
                    result.AddRange(p.Members.Where(x => x.DutyStatus.IsExceptionToNormalDuty == true).ToList());
                }
            }            
            return result;
        }
    }
}
