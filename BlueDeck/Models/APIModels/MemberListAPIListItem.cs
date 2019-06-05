using BlueDeck.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    public class MemberListAPIListItem
    {
        public string Name { get; set; }
        public int BlueDeckId { get; set; }
        public string OrgId {get;set;}

        public MemberListAPIListItem()
        {
        }

        public MemberListAPIListItem(MemberSelectListItem _item)
        {
            Name = _item.MemberName;
            BlueDeckId = _item.MemberId;
        }

        public MemberListAPIListItem(Member _member)
        {
            Name = _member.GetTitleName();
            BlueDeckId = _member.MemberId;
            OrgId = _member.IdNumber;
        }
    }
}
