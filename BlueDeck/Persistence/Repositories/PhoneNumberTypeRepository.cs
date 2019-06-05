using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Types;
using System.Collections.Generic;
using System.Linq;

namespace BlueDeck.Persistence.Repositories
{
    /// <summary>
    /// An implementation of the <see cref="IPhoneNumberTypeRepository"/> that handles CRUD interactions for the <see cref="PhoneNumberType"/> entities.
    /// </summary>
    /// <seealso cref="Repository{PhoneNumberType}" />
    /// <seealso cref="IPhoneNumberTypeRepository" />
    public class PhoneNumberTypeRepository : Repository<PhoneNumberType>, IPhoneNumberTypeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneNumberTypeRepository"/> class.
        /// </summary>
        /// <param name="context">An injected <see cref="ApplicationDbContext"/> database context obtained from the services middleware.</param>
        public PhoneNumberTypeRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets a list of <see cref="PhoneNumberTypeSelectListItem" /> items for all phone number types in the database.
        /// </summary>
        /// <returns>
        /// A <see cref="List{PhoneNumberTypeSelectListItem}" />
        /// </returns>
        /// <remarks>
        /// This method is used to populate select lists for Phone Number Types
        /// </remarks>
        public List<PhoneNumberTypeSelectListItem> GetPhoneNumberTypeSelectListItems()
        {
            return GetAll().ToList().ConvertAll(x => new PhoneNumberTypeSelectListItem(x));
        }

        /// <summary>
        /// Gets a list of all <see cref="PhoneNumberType" /> entities including all associated <see cref="ContactNumber" />
        /// </summary>
        /// <returns>
        /// A <see cref="List{PhoneNumberType}"/> that includes all <see cref="PhoneNumberType"/> entities and their related <see cref="ContactNumber"/>.
        /// </returns>
        public List<PhoneNumberType> GetPhoneNumberTypesWithPhoneNumbers()
        {
            return ApplicationDbContext.PhoneNumberTypes.Include(x => x.ContactNumbers).ToList();
        }

        /// <summary>
        /// Gets a specific <see cref="PhoneNumberType" /> entity including all associated <see cref="ContactNumber" />
        /// </summary>
        /// <param name="id">The identity of the <see cref="PhoneNumberType" /></param>
        /// <returns>
        /// A <see cref="PhoneNumberType"/> object.
        /// </returns>
        public PhoneNumberType GetPhoneNumberTypeWithPhoneNumbers(int id)
        {
            return ApplicationDbContext.PhoneNumberTypes.Include(x => x.ContactNumbers).First(x => x.PhoneNumberTypeId == id);
        }
        /// <summary>
        /// Exposes the injected DB Context to class methods.
        /// </summary>
        /// <value>
        /// The application database context.
        /// </value>
        public ApplicationDbContext ApplicationDbContext {
            get { return Context as ApplicationDbContext; }
        }
    }
}
