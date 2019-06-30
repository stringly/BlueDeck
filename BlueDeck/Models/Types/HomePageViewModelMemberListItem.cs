using System;
using System.Linq;

namespace BlueDeck.Models.Types
{
    /// <summary>
    /// Class that represents a Member or Position Item on the "My Coworkers" lists on the Home/Index view
    /// </summary>
    public class HomePageViewModelMemberListItem
    {
        /// <summary>
        /// Gets or sets the member identifier.
        /// </summary>
        /// <value>
        /// The member identifier.
        /// </value>
        public int MemberId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the member.
        /// </summary>
        /// <value>
        /// The display name of the member.
        /// </value>
        public string MemberDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the member rank image URI.
        /// </summary>
        /// <value>
        /// The member rank image URI.
        /// </value>
        public string MemberRankImageUri { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string PositionName { get; set; }
        public int PositionId { get; set; }
        public int ParentComponentId { get; set; }
        public int? AssignedVehicleId { get; set; }
        public string AssignedVehicleNumber { get; set; }
        public string ParentComponentName {get;set;}
        public string TempPositionName { get; set; }
        public int? TempPositionId { get; set; }
        public int? TempParentComponentId {get;set;}
        public string TempParentComponentName {get;set;}
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
            ParentComponentId = member.Position.ParentComponentId;
            ParentComponentName = member.Position.ParentComponent.Name;
            TempPositionName = member?.TempPosition?.Name;
            TempPositionId = member?.TempPositionId;
            TempParentComponentId = member?.TempPosition?.ParentComponentId;
            TempParentComponentName = member.TempPosition?.ParentComponent.Name;
            LineupPosition = member.Position.LineupPosition;
            DutyStatus = member?.DutyStatus?.DutyStatusName ?? "-";
            IsExceptionToNormalDuty = member?.DutyStatus.IsExceptionToNormalDuty ?? false;
            Gender = member.Gender.Abbreviation;
            Race = member.Race.Abbreviation;
            AssignedVehicleId = member?.AssignedVehicle?.VehicleId ?? null;
            AssignedVehicleNumber = member?.AssignedVehicle?.CruiserNumber ?? "No Cruiser";
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
            AssignedVehicleNumber = "-";
            AssignedVehicleId = null;

        }
    }
}
