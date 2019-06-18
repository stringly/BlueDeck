using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Types;
using System.Collections.Generic;
using System.Linq;
using BlueDeck.Models.Enums;

namespace BlueDeck.Persistence.Repositories
{
    public class AppStatusRepository : Repository<AppStatus>, IAppStatusRepository
    {
        public AppStatusRepository(ApplicationDbContext context) : base(context)
        {
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
        public List<ApplicationStatusSelectListItem> GetApplicationStatusSelectListItems()
        {
            return GetAll().ToList().ConvertAll(x => new ApplicationStatusSelectListItem(x));
        }

        public List<AppStatus> GetAppStatusesWithMemberCount()
        {
            return ApplicationDbContext.ApplicationStatuses.Include(x => x.Members).ToList();
        }

        public AppStatus GetAppStatusWithMemberCount(int id)
        {
            return ApplicationDbContext.ApplicationStatuses.Include(x => x.Members).First(x => x.AppStatusId == id);
        }
    }
}
