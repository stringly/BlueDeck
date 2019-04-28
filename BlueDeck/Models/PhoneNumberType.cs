using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrgChartDemo.Models
{
    public class PhoneNumberType
    {
        [Key]
        public int? PhoneNumberTypeId { get; set; }
        [Display(Name = "Phone Type Name")]
        public string PhoneNumberTypeName { get; set; }

        public virtual IEnumerable<ContactNumber> ContactNumbers { get; set; }
    }
}
