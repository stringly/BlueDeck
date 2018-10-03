using System.Collections.Generic;
using OrgChartDemo.Models.Types;
using System.ComponentModel.DataAnnotations;


namespace OrgChartDemo.Models.ViewModels
{
    /// <summary>
    /// ViewModel used to display a Position and populate a selectlist of Component Names/Ids to facilitate adding a Position or changing the Component to which a position is assigned. 
    /// </summary>
    public class MemberWithPositionListViewModel
    {
        /// <summary>
        /// Gets or sets the Id of the Member.
        /// </summary>
        /// <value>
        /// The Member's Id.
        /// </value>
        public int? MemberId { get; set; }

        /// <summary>
        /// Gets or sets the Rank of the Member.
        /// </summary>
        public int? MemberRank { get; set; }

        /// <summary>
        /// Gets or sets the First Name of the Member.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [StringLength(50), Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last Name of the Member.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [StringLength(50), Required]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Middle Name of the Member.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        [StringLength(50), Required]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the Member's IDNumber.
        /// </summary>
        /// <remarks>
        /// This is NOT the Member Entity Id. This is the Member's employee Id number
        /// </remarks>
        /// <value>
        /// The identifier number.
        /// </value>
        [StringLength(50), Required]
        public string IdNumber { get; set; }

        /// <summary>
        /// Gets or sets the Member's email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [StringLength(50), Required]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Position identifier.
        /// </summary>
        /// <value>
        /// The position identifier.
        /// </value>
        public int? PositionId { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="T:OrgChartDemo.Models.Types.MemberRankSelectListItem"/>s.
        /// </summary>
        /// <value>
        /// The rank list.
        /// </value>
        public List<MemberRankSelectListItem> RankList { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="T:OrgChartDemo.Models.Types.PositionSelectListItem"/>.
        /// </summary>
        /// <value>
        /// The positions.
        /// </value>
        public List<PositionSelectListItem> Positions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.ViewModels.MemberWithPositionListViewModel"/> class.
        /// <remarks>
        /// This parameter-less constructor had to be added in because the <see cref="T:OrgChartDemo.ViewModels.MemberWithPositionListViewModel(Member, List{Position})"/> constructor overrode the default, and the form POST model-binding failed
        /// </remarks>
        /// </summary>
        public MemberWithPositionListViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberWithPositionListViewModel"/> class.
        /// </summary>
        /// <param name="m">A <see cref="T:OrgChartDemo.Models.Member"/>.</param>
        /// <param name="l">A <see cref="T:List{OrgChartDemo.Models.Position}"/>.</param>
        /// <param name="r">A <see cref="T:List{OrgChartDemo.Models.Types.MemberRankSelectListItem}}"/>.</param>
        public MemberWithPositionListViewModel(Member m, List<Position> l, List<MemberRankSelectListItem> r)
        {
            MemberId = m?.MemberId;
            MemberRank = m?.Rank?.RankId;
            FirstName = m.FirstName;
            LastName = m.LastName;
            MiddleName = m.MiddleName;
            IdNumber = m.IdNumber;
            Email = m.Email;
            PositionId = m?.Position?.PositionId;
            RankList = r;
            Positions = l.ConvertAll(x => new PositionSelectListItem { PositionId = x.PositionId, PositionName = x.Name });

        }
    }
}
