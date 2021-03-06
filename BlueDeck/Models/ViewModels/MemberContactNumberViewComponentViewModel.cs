﻿using BlueDeck.Models.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.ViewModels
{
    public class MemberContactNumberViewComponentViewModel
    {
        [Display(Name = "Contact Numbers")]
        public List<ContactNumber> ContactNumbers { get; set; }
        public List<PhoneNumberTypeSelectListItem> NumberTypeList { get; set; }
        public int MemberId { get; set; }

        public MemberContactNumberViewComponentViewModel()
        {
        }
        public MemberContactNumberViewComponentViewModel(Member m, List<PhoneNumberTypeSelectListItem> types)
        {
            ContactNumbers = m.PhoneNumbers.ToList();
            NumberTypeList = types;
            MemberId = m.MemberId;
        }
    }
}
