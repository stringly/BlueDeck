using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Types;

namespace BlueDeck.Persistence.Repositories
{
    /// <summary>
    /// A repository for the MemberGender entity.
    /// </summary>
    /// <seealso cref="T:BlueDeck.Persistence.Repositories.Repository{BlueDeck.Models.Types.MemberGender}" />
    /// <seealso cref="T:BlueDeck.Models.Repositories.IMemberGenderRepository" />
    public class MemberGenderRepository: Repository<Gender>, IMemberGenderRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberGenderRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="T:BlueDeck.Models.ApplicationDbContext"/></param>
        public MemberGenderRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets a list of <see cref="T:BlueDeck.Types.MemberGenderSelectListItem" />s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Gender select lists.
        /// </remarks>
        /// <returns>
        /// A <see cref="T:List{BlueDeck.Models.Types.MemberGenderSelectListItem}" />
        /// </returns>
        public List<MemberGenderSelectListItem> GetMemberGenderSelectListItems()
        {
            return GetAll().ToList().ConvertAll(x => new MemberGenderSelectListItem { MemberGenderId = System.Convert.ToInt32(x.GenderId), MemberGenderFullName = x.GenderFullName , Abbreviation = x.Abbreviation });
        }

        /// <summary>
        /// Gets the gender by identifier.
        /// </summary>
        /// <param name="memberGenderId">The <see cref="Gender" /> identifier.</param>
        /// <returns>
        /// A <see cref="Gender" />
        /// </returns>
        public Gender GetGenderById(int memberGenderId)
        {
            return ApplicationDbContext.Genders
                .Where(x => x.GenderId == memberGenderId)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets a list of all <see cref="Gender"/> with members.
        /// </summary>
        /// <returns>A <see cref="List{Gender}"/> containing all current <see cref="Gender"/></returns>
        public List<Gender> GetGendersWithMembers()
        {
            return ApplicationDbContext.Genders.Include(x => x.Members).ToList();
        }

        /// <summary>
        /// Gets a <see cref="Gender"/> with it's current <see cref="Member"/>s.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="Gender"/>.</param>
        /// <returns>A <see cref="Gender"/></returns>
        public Gender GetGenderWithMembers(int id)
        {
            return ApplicationDbContext.Genders.Include(x => x.Members).First(x => x.GenderId == id);
        }

        /// <summary>
        /// Gets the application database context.
        /// </summary>
        /// <value>
        /// The application database context.
        /// </value>
        public ApplicationDbContext ApplicationDbContext {
            get { return Context as ApplicationDbContext; }
        }
    }
}
