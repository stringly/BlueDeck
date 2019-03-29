using OrgChartDemo.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrgChartDemo.Models
{
    public class ContactNumber
    {
        [Key]
        public int MemberContactNumberId {get;set;}

        public PhoneNumberType Type {get;set;}

        public string PhoneNumber {get;set;}

        public int MemberId { get; set; }
        public virtual Member Member {get;set;}

        [NotMapped]
        public bool ToDelete { get; set; }

    }
}
