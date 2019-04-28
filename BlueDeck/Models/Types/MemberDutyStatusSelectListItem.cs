using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Types
{
    /// <summary>
    /// A Type that includes the DutyStatusId and DutyStatusName for the <see cref="T:BlueDeck.Models.Types.MemberDutyStatus"/> Entity.
    /// /// <remarks>
    /// This type is used to populate a MemberDutyStatus select list.
    /// </remarks>
    /// </summary>
    public class MemberDutyStatusSelectListItem
    {
        /// <summary>
        /// Gets or sets the member duty status identifier.
        /// </summary>
        /// <value>
        /// The member duty status identifier.
        /// </value>
        public int MemberDutyStatusId { get; set; }

        /// <summary>
        /// Gets or sets the name of the member duty status.
        /// </summary>
        /// <value>
        /// The name of the member duty status.
        /// </value>
        public string MemberDutyStatusName { get; set; }
    }
}
