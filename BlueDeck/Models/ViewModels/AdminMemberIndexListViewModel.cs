﻿using BlueDeck.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.ViewModels
{
    public class AdminMemberIndexListViewModel
    {
        public IEnumerable<AdminMemberIndexViewModelListItem> Members { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string MemberFirstNameSort { get; set; }
        public string MemberLastNameSort { get; set; }
        public string IdNumberSort { get; set; }
        public string PositionNameSort { get; set; }
        public string IsUserRoleFilter { get; set; }
        public string IsComponentAdminRoleFilter { get; set; }
        public string IsGlobalAdminRoleFilter { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
                
        public AdminMemberIndexListViewModel()
        {
            Members = new List<AdminMemberIndexViewModelListItem>();
        }

        public AdminMemberIndexListViewModel(List<Member> members)
        {
            Members = members.ConvertAll(x => new AdminMemberIndexViewModelListItem(x));
        }
    }
}
