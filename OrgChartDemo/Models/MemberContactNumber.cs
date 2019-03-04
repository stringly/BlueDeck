using OrgChartDemo.Models.Types;
using System.ComponentModel.DataAnnotations;

namespace OrgChartDemo.Models
{
    public class MemberContactNumber
    {
        [Key]
        public int MemberContactNumberId {get;set;}

        public PhoneNumberType Type {get;set;}

        public string PhoneNumber {get;set;}

        public Member Member {get;set;}

    }
}
