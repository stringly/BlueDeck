using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Types
{
    /// <summary>
    /// A Type that includes the PositionName and PositionId for the <see cref="T:OrgChartDemo.Models.Position"/> Entity.
    /// <remarks>
    /// This type is used to populate a Position select list.
    /// </remarks>
    /// </summary>
    public class PositionSelectListItem
    {
        /// <summary>
        /// Gets or sets the Rank Name
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// Gets or sets the Rank Id
        /// </summary>
        public int PositionId { get; set; }

        public PositionSelectListItem()
        {
        }

        public PositionSelectListItem(Position _p)
        {
            PositionName = _p.Name;
            PositionId = _p.PositionId;
        }
    }
}
