
namespace OrgChartDemo.Models.Types
{
    /// <summary>
    /// A Type that includes the MemberRaceId and RaceFullName for the <see cref="T:OrgChartDemo.Models.Types.MemberGender"/> Entity.
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
    }
}
