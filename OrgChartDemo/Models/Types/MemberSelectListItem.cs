using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Types
{
    /// <summary>
    /// A Type that includes the MemberDisplayName and MemberId for the <see cref="T:OrgChartDemo.Models.Member"/> Entity.
    /// <remarks>
    /// This type is used to populate a Position select list.
    /// </remarks>
    /// </summary>
    public class MemberSelectListItem
    {
        /// <summary>
        /// Gets or sets the Member Name
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets the Member Id
        /// </summary>
        public int MemberId { get; set; }
        
        public MemberSelectListItem()
        {
        }
        public MemberSelectListItem(Member m)
        {
            MemberName = m.GetTitleName();
            MemberId = m.MemberId;
        }
    }
}

