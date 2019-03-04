using OrgChartDemo.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrgChartDemo.Models
{
    public class MemberContactNumber
    {
        [Key]
        public int MemberContactNumberId {get;set;}

        public PhoneNumberType Type {get;set;}

        public string PhoneNumber {get;set;}

        public Member Member {get;set;}

        [NotMapped]
        public bool ToDelete { get; set; }

    }
}
