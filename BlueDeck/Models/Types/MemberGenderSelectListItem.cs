using System;
using BlueDeck.Models.Enums;

namespace BlueDeck.Models.Types
{
    /// <summary>
    /// A Type that includes the MemberRaceId and RaceFullName for the <see cref="T:BlueDeck.Models.Types.MemberGender"/> Entity.
    /// /// <remarks>
    /// This type is used to populate a MemberGender select list.
    /// </remarks>
    /// </summary>
    public class MemberGenderSelectListItem
    {
        /// <summary>
        /// Gets or sets the member gender identifier.
        /// </summary>
        /// <value>
        /// The member gender identifier.
        /// </value>
        public int MemberGenderId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the member gender.
        /// </summary>
        /// <value>
        /// The full name of the member gender.
        /// </value>
        public string MemberGenderFullName { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation.
        /// </summary>
        /// <value>
        /// The abbreviation.
        /// </value>
        public char Abbreviation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberGenderSelectListItem"/> class.
        /// </summary>
        public MemberGenderSelectListItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberGenderSelectListItem"/> class.
        /// </summary>
        /// <param name="_g">The g.</param>
        public MemberGenderSelectListItem(Gender _g)
        {
            MemberGenderId = Convert.ToInt32(_g.GenderId);
            MemberGenderFullName = _g.GenderFullName;
            Abbreviation = _g.Abbreviation;
        }
    }
}
