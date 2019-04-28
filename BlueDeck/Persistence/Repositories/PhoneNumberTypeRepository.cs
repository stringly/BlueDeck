using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;
using System.Collections.Generic;
using System.Linq;

namespace OrgChartDemo.Persistence.Repositories
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
