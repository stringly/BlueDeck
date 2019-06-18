using BlueDeck.Models.Enums;
using System;

namespace BlueDeck.Models.Types
{
    /// <summary>
    /// A Type that includes the MemberRaceId and RaceFullName for the <see cref="T:BlueDeck.Models.Types.MemberRace"/> Entity.
    /// /// <remarks>
    /// This type is used to populate a MemberRace select list.
    /// </remarks>
    /// </summary>
    public class MemberRaceSelectListItem
    {
        /// <summary>
        /// Gets or sets the member race identifier.
        /// </summary>
        /// <value>
        /// The member race identifier.
        /// </value>
        public int MemberRaceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the race.
        /// </summary>
        /// <value>
        /// The name of the race.
        /// </value>
        public string RaceFullName { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation.
        /// </summary>
        /// <value>
        /// The abbreviation.
        /// </value>
        public char Abbreviation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRaceSelectListItem"/> class.
        /// </summary>
        public MemberRaceSelectListItem()
        {
        }
        
        public MemberRaceSelectListItem(Race _r)
        {
            MemberRaceId = Convert.ToInt32(_r.MemberRaceId);
            RaceFullName = _r.MemberRaceFullName;
            Abbreviation = _r.Abbreviation;

        }

    }
}
