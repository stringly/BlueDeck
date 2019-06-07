using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    /// <summary>
    /// Class that represents a <see cref="Member"/> entity in a WebAPI response
    /// </summary>
    public class MemberApiResult
    {
        /// <summary>
        /// Gets or sets the member identifier.
        /// </summary>
        /// <value>
        /// The member identifier.
        /// </value>
        public int MemberId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the PGPD identifier.
        /// </summary>
        /// <remarks>
        /// This value maps to the <see cref="Member.IdNumber"/>
        /// </remarks>
        /// <value>
        /// The PGPD identifier.
        /// </value>
        public string PGPDId { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Member's contact numbers.
        /// </summary>
        /// <remarks>
        /// This is a list of <see cref="ContactNumberApiResult"/> objects created from the <see cref="Member.PhoneNumbers"/> property
        /// </remarks>
        /// <value>
        /// The contact numbers.
        /// </value>
        public IEnumerable<ContactNumberApiResult> ContactNumbers { get; set; }

        /// <summary>
        /// Gets or sets the Member's rank.
        /// </summary>
        /// <value>
        /// <remarks>
        /// This is a <see cref="RankApiResult"/> object.
        /// </remarks>
        /// The rank.
        /// </value>
        public RankApiResult Rank { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <remarks>
        /// This is a <see cref="GenderApiResult"/> object.
        /// </remarks>
        /// <value>
        /// The gender.
        /// </value>
        public GenderApiResult Gender { get; set; }

        /// <summary>
        /// Gets or sets the Member's race.
        /// </summary>
        /// <remarks>
        /// This is a <see cref="RaceApiResult"/> object.
        /// </remarks>
        /// <value>
        /// The race.
        /// </value>
        public RaceApiResult Race { get; set; }

        /// <summary>
        /// Gets or sets the duty status.
        /// </summary>
        /// <value>
        /// <remarks>
        /// This is a <see cref="DutyStatusApiResult"/> object.
        /// </remarks>
        /// The duty status.
        /// </value>
        public DutyStatusApiResult DutyStatus { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// <remarks>
        /// This is a <see cref="SubPositionApiResult"/> object.
        /// </remarks>
        /// The position.
        /// </value>
        public SubPositionApiResult Position { get; set; }

        /// <summary>
        /// Gets or sets the supervisor.
        /// </summary>
        /// <remarks>
        /// This is a <see cref="SubPositionApiResult"/> object.
        /// </remarks>
        /// <value>
        /// The supervisor.
        /// </value>
        public SubMemberApiResult Supervisor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberApiResult"/> class.        
        /// </summary>
        /// <remarks>
        /// Default, parameterless constructor.
        /// </remarks>
        public MemberApiResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberApiResult"/> class.
        /// </summary>
        /// <param name="_member">A <see cref="Member"/> object.</param>
        public MemberApiResult(Member _member)
        {
            MemberId = _member.MemberId;
            FirstName = _member.FirstName;
            LastName = _member.LastName;
            PGPDId = _member.IdNumber;
            Email = _member.Email;
            ContactNumbers = _member.PhoneNumbers.ToList().ConvertAll(x => new ContactNumberApiResult(x));
            Rank = new RankApiResult(_member.Rank);
            Gender = new GenderApiResult(_member.Gender);
            Race = new RaceApiResult(_member.Race);
            DutyStatus = new DutyStatusApiResult(_member.DutyStatus);
            Position = new SubPositionApiResult(_member.Position);            
        }
    }
}
