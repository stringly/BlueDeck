
namespace OrgChartDemo.Models.Types
{
    public class PhoneNumberTypeSelectListItem
    {
        public int PhoneNumberTypeId { get; set; }
        public string PhoneNumberTypeName { get; set; }

        public PhoneNumberTypeSelectListItem()
        {
        }
        public PhoneNumberTypeSelectListItem(PhoneNumberType t)
        {
            PhoneNumberTypeId = System.Convert.ToInt32(t.PhoneNumberTypeId);
            PhoneNumberTypeName = t.PhoneNumberTypeName;
        }
    }
}
