using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Types;
using System.Collections.Generic;
using System.Linq;
using BlueDeck.Models.Enums;

namespace BlueDeck.Persistence.Repositories
{
    /// <summary>
    /// Repository for CRUD actions for the Application Status Enumeration
    /// </summary>
    /// <seealso cref="Repository{AppStatus}" />
    /// <seealso cref="IAppStatusRepository" />
    public class AppStatusRepository : Repository<AppStatus>, IAppStatusRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppStatusRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
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

        /// <summary>
        /// Gets the application status select list items.
        /// </summary>
        /// <returns></returns>
        public List<ApplicationStatusSelectListItem> GetApplicationStatusSelectListItems()
        {
            return GetAll().ToList().ConvertAll(x => new ApplicationStatusSelectListItem(x));
        }

        /// <summary>
        /// Gets the application statuses with member count.
        /// </summary>
        /// <returns></returns>
        public List<AppStatus> GetAppStatusesWithMemberCount()
        {
            return ApplicationDbContext.ApplicationStatuses.Include(x => x.Members).ToList();
        }

        /// <summary>
        /// Gets the application status with member count.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public AppStatus GetAppStatusWithMemberCount(int id)
        {
            return ApplicationDbContext.ApplicationStatuses.Include(x => x.Members).First(x => x.AppStatusId == id);
        }
    }
}
