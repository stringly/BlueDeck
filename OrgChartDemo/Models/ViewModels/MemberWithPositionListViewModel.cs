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

        [StringLength(50), Required]
        public string FirstName { get; set; }
        [StringLength(50), Required]
        public string LastName { get; set; }
        [StringLength(50), Required]
        public string MiddleName { get; set; }
        [StringLength(50), Required]
        public string IdNumber { get; set; }
        [StringLength(50), Required]
        public string Email { get; set; }
        public int? PositionId { get; set; }

        public List<MemberRankSelectListItem> RankList { get; set; }
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
