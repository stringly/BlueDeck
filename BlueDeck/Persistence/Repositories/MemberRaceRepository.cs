﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Types;

namespace BlueDeck.Persistence.Repositories
{
    /// <summary>
    /// A repository for the MemberRace entity.
    /// </summary>
    /// <seealso cref="T:BlueDeck.Persistence.Repositories.Repository{BlueDeck.Models.Types.MemberRace}" />
    /// <seealso cref="T:BlueDeck.Models.Repositories.IMemberRaceRepository" />
    public class MemberRaceRepository : Repository<Race>, IMemberRaceRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:BlueDeck.Persistence.Repositories.MemberRaceRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="T:BlueDeck.Models.ApplicationDbContext"/></param>
        public MemberRaceRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Gets a list of <see cref="T:BlueDeck.Types.MemberRaceSelectListItem" />s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Rank select lists.
        /// </remarks>
        /// <returns>
        /// A <see cref="T:List{BlueDeck.Models.Types.MemberRaceSelectListItem}" />
        /// </returns>
        public List<MemberRaceSelectListItem> GetMemberRaceSelectListItems()
        {
            return GetAll().ToList().ConvertAll(x => new MemberRaceSelectListItem { MemberRaceId = System.Convert.ToInt32(x.MemberRaceId), RaceFullName = x.MemberRaceFullName, Abbreviation = x.Abbreviation });
        }

        public Race GetRaceById(int memberRaceId)
        {
            return ApplicationDbContext.Races
                .Where(x => x.MemberRaceId == memberRaceId)
                .FirstOrDefault();
        }

        public List<Race> GetRacesWithMembers()
        {
            return ApplicationDbContext.Races.Include(x => x.Members).ToList();
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