using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Persistence.Repositories
{
    /// <summary>
    /// A repository for the MemberGender entity.
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Persistence.Repositories.Repository{OrgChartDemo.Models.Types.MemberGender}" />
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IMemberGenderRepository" />
    public class MemberGenderRepository: Repository<MemberGender>, IMemberGenderRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberGenderRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/></param>
        public MemberGenderRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets a list of <see cref="T:OrgChartDemo.Types.MemberGenderSelectListItem" />s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Gender select lists.
        /// </remarks>
        /// <returns>
        /// A <see cref="T:List{OrgChartDemo.Models.Types.MemberGenderSelectListItem}" />
        /// </returns>
        public List<MemberGenderSelectListItem> GetMemberGenderSelectListItems()
        {
            return GetAll().ToList().ConvertAll(x => new MemberGenderSelectListItem { MemberGenderId = x.GenderId, MemberGenderFullName = x.GenderFullName , Abbreviation = x.Abbreviation });
        }
    }
}
