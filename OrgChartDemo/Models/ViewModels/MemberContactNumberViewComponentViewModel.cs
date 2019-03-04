using OrgChartDemo.Models.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.ViewModels
{
    public class MemberContactNumberViewComponentViewModel
    {
        [Display(Name = "Contact Numbers")]
        public List<MemberContactNumber> ContactNumbers { get; set; }
        public List<PhoneNumberTypeSelectListItem> NumberTypeList { get; set; }
        public int MemberId { get; set; }

        public MemberContactNumberViewComponentViewModel()
        {
        }
        public MemberContactNumberViewComponentViewModel(Member m, List<PhoneNumberTypeSelectListItem> types)
        {
            ContactNumbers = m.PhoneNumbers;
            NumberTypeList = types;
            MemberId = m.MemberId;
        }
    }
}
