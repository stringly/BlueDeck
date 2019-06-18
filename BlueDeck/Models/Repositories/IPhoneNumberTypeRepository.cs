using BlueDeck.Models.Types;
using System.Collections.Generic;
using BlueDeck.Models.Enums;

namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// Repository that handles database interactions for the <see cref="PhoneNumberType"/> entity.
    /// </summary>
    /// <seealso cref="IRepository{PhoneNumberType}" />
    public interface IPhoneNumberTypeRepository : IRepository<PhoneNumberType>
    {
        /// <summary>
        /// Gets a list of <see cref="PhoneNumberTypeSelectListItem"/> items for all phone number types in the database.
        /// </summary>
        /// <remarks>
        /// This method is used to populate select lists for Phone Number Types
        /// </remarks>
        /// <returns>A <see cref="List{PhoneNumberTypeSelectListItem}"/></returns>
        List<PhoneNumberTypeSelectListItem> GetPhoneNumberTypeSelectListItems();

        /// <summary>
        /// Gets a list of all <see cref="PhoneNumberType"/> entities including all associated <see cref="ContactNumber"/>
        /// </summary>
        /// <returns>A <see cref="List{PhoneNumberType}"/></returns>
        List<PhoneNumberType> GetPhoneNumberTypesWithPhoneNumbers();

        /// <summary>
        /// Gets a specific <see cref="PhoneNumberType"/> entity including all associated <see cref="ContactNumber"/>
        /// </summary>
        /// <param name="id">The identity of the <see cref="PhoneNumberType"/></param>
        /// <returns>A <see cref="PhoneNumberType"/> object.</returns>
        PhoneNumberType GetPhoneNumberTypeWithPhoneNumbers(int id);
    }
}
