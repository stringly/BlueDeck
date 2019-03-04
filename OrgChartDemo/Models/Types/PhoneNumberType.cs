using System.ComponentModel.DataAnnotations;

namespace OrgChartDemo.Models.Types
{
    public class PhoneNumberType
    {
        [Key]
        public int PhoneNumberTypeId { get; set; }
        public string PhoneNumberTypeName { get; set; }
    }
}
