using BlueDeck.Models.Types;
using System.Collections.Generic;

namespace BlueDeck.Models.Repositories
{
    public interface IPhoneNumberTypeRepository : IRepository<PhoneNumberType>
    {
        List<PhoneNumberTypeSelectListItem> GetPhoneNumberTypeSelectListItems();
        List<PhoneNumberType> GetPhoneNumberTypesWithPhoneNumbers();
    }
}
