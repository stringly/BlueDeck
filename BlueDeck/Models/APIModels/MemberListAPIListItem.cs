using BlueDeck.Models.Types;

namespace BlueDeck.Models.APIModels
{
    /// <summary>
    /// Class that represents a <see cref="Member"/> entity when used as part of a list result for a WebAPI request.
    /// </summary>
    public class MemberListAPIListItem
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <remarks>
        /// The name is used as a display in a list, so it should be a "Display" name derived from the <see cref="Member"/> Rank, First Name, Last Name, and ID Number.
        /// </remarks>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the blue deck identifier.
        /// </summary>
        /// <remarks>
        /// This is the <see cref="Member.MemberId"/>. It is called "BlueDeck ID" here because it is used in a WebAPI.
        /// </remarks>
        /// <value>
        /// The blue deck identifier.
        /// </value>
        public int BlueDeckId { get; set; }

        /// <summary>
        /// Gets or sets the org identifier.
        /// </summary>
        /// <remarks>
        /// This is mapped to the <see cref="Member.IdNumber"/>, which is the Member's organization-unique ID (distinct from the BlueDeckID)
        /// </remarks>
        /// <value>
        /// The org identifier.
        /// </value>
        public string OrgId {get;set;}

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberListAPIListItem"/> class.
        /// </summary>
        /// <remarks>
        /// Default constructor that takes no parameters.
        /// </remarks>
        public MemberListAPIListItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberListAPIListItem"/> class.
        /// </summary>
        /// <remarks>
        /// The class constructor that can take a <see cref="MemberSelectListItem"/> as a constructor.
        /// <see cref="MemberListAPIListItem"/> objects constructed from <see cref="MemberSelectListItem"/> will have a nullstring "OrgID" property.
        /// </remarks>
        /// <param name="_item">A <see cref="MemberSelectListItem"/></param>
        public MemberListAPIListItem(MemberSelectListItem _item)
        {
            Name = _item.MemberName;
            BlueDeckId = _item.MemberId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberListAPIListItem"/> class.
        /// </summary>
        /// <remarks>
        /// The class constructor that can take a <see cref="Member"/> parameter.
        /// </remarks>
        /// <param name="_member">A <see cref="Member"/> object.</param>
        public MemberListAPIListItem(Member _member)
        {
            Name = _member.GetTitleName();
            BlueDeckId = _member.MemberId;
            OrgId = _member.IdNumber;
        }
    }
}
