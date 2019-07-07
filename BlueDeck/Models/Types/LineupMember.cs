using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueDeck.Models.Enums;

namespace BlueDeck.Models.Types
{
    /// <summary>
    /// Class that represents a Member entity in the Lineup Generator view
    /// </summary>
    public class LineupMember
    {
        /// <summary>
        /// Gets or sets the callsign.
        /// </summary>
        /// <value>
        /// The callsign.
        /// </value>
        public string Callsign { get; set; }

        /// <summary>
        /// Gets or sets the bedge number.
        /// </summary>
        /// <value>
        /// The bedge number.
        /// </value>
        public string BadgeNumber { get; set; }

        /// <summary>
        /// Gets or sets the rank.
        /// </summary>
        /// <value>
        /// The rank.
        /// </value>
        public string Rank { get; set; }

        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        /// <value>
        /// The name of the member.
        /// </value>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets the leave status.
        /// </summary>
        /// <value>
        /// The leave status.
        /// </value>
        public string StatusNote { get; set; }

        /// <summary>
        /// Gets or sets the MVS status.
        /// </summary>
        /// <value>
        /// The MVS status.
        /// </value>
        public int? MVSStatus { get; set; }

        /// <summary>
        /// Gets or sets the cruiser number.
        /// </summary>
        /// <value>
        /// The cruiser number.
        /// </value>
        public string CruiserNumber { get; set; }

        /// <summary>
        /// Gets or sets the shift working.
        /// </summary>
        /// <value>
        /// The shift working.
        /// </value>
        public int ShiftWorking { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is overlap.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is overlap; otherwise, <c>false</c>.
        /// </value>
        public bool IsOverlap { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int StatusId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineupMember"/> class.
        /// </summary>
        public LineupMember()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineupMember"/> class.
        /// </summary>
        /// <param name="_member">The member.</param>
        public LineupMember(Member _member)
        {
            if (_member != null)
            {
                if (_member.Position.Callsign != null && _member.Position.Callsign != "NONE")
                {
                    Callsign = _member.Position.Callsign;
                }
                else if (_member.TempPosition != null)
                {
                    if (_member.TempPosition.Callsign != null && _member.TempPosition.Callsign != "NONE")
                    {
                        Callsign =  _member.TempPosition.Callsign;

                    }
                }
                
                BadgeNumber = _member.IdNumber;
                Rank = _member.Rank.RankShort;
                MemberName = _member.GetLastNameFirstName().ToUpper();
                CruiserNumber = _member?.AssignedVehicle?.CruiserNumber;
                if (_member.AssignedVehicle != null)
                {
                    if (_member.AssignedVehicle.HasMVS == true)
                    {
                        MVSStatus = 1;
                    }
                    else
                    {
                        MVSStatus = 0;
                    }
                }
                else
                {
                    MVSStatus = null;
                }
                switch (_member.DutyStatus.DutyStatusName)
                {
                    case "Light Duty":
                        StatusId = 9;
                        StatusNote = "Light Duty";
                        break;
                    case "No Duty":
                        StatusId = 10;
                        StatusNote = "No Duty";
                        break;
                    case "Suspended":
                        StatusId = 11;
                        StatusNote = "Suspended";
                        break;
                    case "Military Leave":
                        StatusId = 6;
                        StatusNote = "Military Leave";
                        break;
                    case "FMLA Leave":
                        StatusId = 7;
                        StatusNote = "FMLA Leave";
                        break;
                    default:
                        StatusId = 0;                        
                        break;
                }                
                IsOverlap = false;
            }
            
        }
    }

    /// <summary>
    /// Type used to set the Working Status of a <see cref="LineupMember"/>
    /// </summary>
    public class LineupMemberStatus
    {
        /// <summary>
        /// Gets or sets the status identifier.
        /// </summary>
        /// <value>
        /// The status identifier.
        /// </value>
        public int StatusId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is on duty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is on duty; otherwise, <c>false</c>.
        /// </value>
        public bool IsOnDuty { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineupMemberStatus"/> class.
        /// </summary>
        public LineupMemberStatus()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineupMemberStatus"/> class.
        /// </summary>
        /// <param name="_dutyStatus">The duty status.</param>
        public LineupMemberStatus(DutyStatus _dutyStatus)
        {
            Status = _dutyStatus.DutyStatusName;
            if (_dutyStatus.IsExceptionToNormalDuty || !_dutyStatus.HasPolicePower)
            {
                IsOnDuty = false;
            }
            else
            {
                IsOnDuty = true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineupMemberStatus"/> class.
        /// </summary>
        /// <param name="_statusName">Name of the status.</param>
        /// <param name="_isOnDuty">if set to <c>true</c> [is on duty].</param>
        public LineupMemberStatus(string _statusName, bool _isOnDuty)
        {
            Status = _statusName;
            IsOnDuty = _isOnDuty;
        }
    }
}
