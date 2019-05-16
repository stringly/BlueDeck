using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Types
{
    public class HomePageViewModelMemberListItem
    {
        public int MemberId { get; set; }
        public string MemberDisplayName { get; set; }
        public string MemberRankImageUri { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string PositionName { get; set; }
        public int PositionId { get; set; }
        public int? LineupPosition { get; set; }
        public string DutyStatus { get; set; }
        public bool IsExceptionToNormalDuty { get; set; }
        public char Gender { get; set; }
        public char Race { get; set; }

        public HomePageViewModelMemberListItem(Member member)
        {
            MemberId = member.MemberId;
            MemberDisplayName = member.GetTitleName();
            MemberRankImageUri = member.Rank.GetRankImageSource();
            EmailAddress = member?.Email ?? "-";
            ContactNumber = member?.PhoneNumbers?.FirstOrDefault()?.PhoneNumber ?? "None";
            PositionName = member.Position.Name;
            PositionId = member.Position.PositionId;
            LineupPosition = member.Position.LineupPosition;
            DutyStatus = member?.DutyStatus?.DutyStatusName ?? "-";
            IsExceptionToNormalDuty = member?.DutyStatus.IsExceptionToNormalDuty ?? false;
            Gender = member.Gender.Abbreviation;
            Race = member.Race.Abbreviation;


        }
        public HomePageViewModelMemberListItem(Position p)
        {
            MemberId = 0;
            MemberDisplayName = "Vacant";
            MemberRankImageUri = "";
            EmailAddress = "-";
            ContactNumber = "-";
            PositionName = p.Name;
            PositionId = p.PositionId;
            DutyStatus = "-";
            LineupPosition = p.LineupPosition;

        }
    }
}
