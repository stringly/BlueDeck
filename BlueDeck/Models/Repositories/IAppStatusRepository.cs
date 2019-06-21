using BlueDeck.Models.Types;
using System.Collections.Generic;
using BlueDeck.Models.Enums;

namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// Type that represents Application Statuses for use in Drop-down lists.
    /// </summary>
    /// <seealso cref="IRepository{AppStatus}" />
    public interface IAppStatusRepository : IRepository<AppStatus>
    {
        /// <summary>
        /// Gets the application status select list items.
        /// </summary>
        /// <returns></returns>
        List<ApplicationStatusSelectListItem> GetApplicationStatusSelectListItems();

        /// <summary>
        /// Gets the application statuses with member count.
        /// </summary>
        /// <returns></returns>
        List<AppStatus> GetAppStatusesWithMemberCount();

        /// <summary>
        /// Gets the application status with member count.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        AppStatus GetAppStatusWithMemberCount(int id);
    }
}

