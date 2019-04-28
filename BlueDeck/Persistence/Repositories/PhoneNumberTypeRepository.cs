using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Types;
using System.Collections.Generic;
using System.Linq;

namespace BlueDeck.Persistence.Repositories
{
    public class PhoneNumberTypeRepository : Repository<PhoneNumberType>, IPhoneNumberTypeRepository
    {
        public PhoneNumberTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public List<PhoneNumberTypeSelectListItem> GetPhoneNumberTypeSelectListItems()
        {
            return GetAll().ToList().ConvertAll(x => new PhoneNumberTypeSelectListItem(x));
        }
        public List<PhoneNumberType> GetPhoneNumberTypesWithPhoneNumbers()
        {
            return ApplicationDbContext.PhoneNumberTypes.Include(x => x.ContactNumbers).ToList();
        }
        public ApplicationDbContext ApplicationDbContext {
            get { return Context as ApplicationDbContext; }
        }
    }
}
