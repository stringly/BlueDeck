using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public string LeaveStatus { get; set; }

        /// <summary>
        /// Gets or sets the MVS status.
        /// </summary>
        /// <value>
        /// The MVS status.
        /// </value>
        public int MVSStatus { get; set; }

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
                Callsign = _member.Position.Callsign;
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
                    MVSStatus = 0;
                }
                LeaveStatus = _member.DutyStatus.DutyStatusName == "Full Duty" ? "" :  _member.DutyStatus.DutyStatusName;
                IsOverlap = false;
            }
            
        }
    }
}
