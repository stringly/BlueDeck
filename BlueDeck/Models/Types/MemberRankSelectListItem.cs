using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueDeck.Models.Enums;

namespace BlueDeck.Models.Types
{
    /// <summary>
    /// A Type that includes the MemberRankId and RankFullName for the <see cref="T:BlueDeck.Models.Types.MemberRank"/> Entity.
    /// <remarks>
    /// This type is used to populate a MemberRank select list.
    /// </remarks>
    /// </summary>
    public class MemberRankSelectListItem
    {
        /// <summary>
        /// Gets or sets the member rank identifier.
        /// </summary>
        /// <value>
        /// The member rank identifier.
        /// </value>
        public int MemberRankId { get; set; }

        /// <summary>
        /// Gets or sets the name of the rank.
        /// </summary>
        /// <value>
        /// The name of the rank.
        /// </value>
        public string RankName { get; set; }

        public string RankShort { get; set; }

        public MemberRankSelectListItem()
        {
        }
        public MemberRankSelectListItem(Rank _rank)
        {
            MemberRankId = Convert.ToInt32(_rank.RankId);
            RankName = _rank.RankFullName;
            RankShort = _rank.RankShort;
        }
    }
}
