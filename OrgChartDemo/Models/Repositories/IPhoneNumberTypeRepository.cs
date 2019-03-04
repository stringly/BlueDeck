using OrgChartDemo.Models.Types;
using System.Collections.Generic;

namespace OrgChartDemo.Models.Repositories
{
    public interface IPhoneNumberTypeRepository : IRepository<PhoneNumberType>
    {
        List<PhoneNumberTypeSelectListItem> GetPhoneNumberTypeSelectListItems();
    }
}
