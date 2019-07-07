﻿using BlueDeck.Models.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.ViewModels
{
    /// <summary>
    /// View Model used in the Documents/Lineup view
    /// </summary>
    public class LineupGeneratorViewModel
    {
        /// <summary>
        /// Gets or sets the component identifier.
        /// </summary>
        /// <value>
        /// The component identifier.
        /// </value>
        public int ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the district.
        /// </summary>
        /// <value>
        /// The name of the district.
        /// </value>
        [Display(Name = "District")]
        public string DistrictName { get; set; }

        /// <summary>
        /// Gets or sets the name of the commander.
        /// </summary>
        /// <value>
        /// The name of the commander.
        /// </value>
        public string CommanderName { get; set; }

        /// <summary>
        /// Gets or sets the commander title.
        /// </summary>
        /// <value>
        /// The commander title.
        /// </value>
        public string CommanderTitle { get; set; }

        /// <summary>
        /// Gets or sets the name of the assistant commander.
        /// </summary>
        /// <value>
        /// The name of the assistant commander.
        /// </value>
        public string AssistantCommanderName { get; set; }

        /// <summary>
        /// Gets or sets the assistant commander title.
        /// </summary>
        /// <value>
        /// The assistant commander title.
        /// </value>
        public string AssistantCommanderTitle { get; set; }

        /// <summary>
        /// Gets or sets the name of the component.
        /// </summary>
        /// <value>
        /// The name of the component.
        /// </value>
        [Display(Name = "Squad")]
        public string ComponentName { get; set; }

        /// <summary>
        /// Gets or sets the lineup date.
        /// </summary>
        /// <value>
        /// The lineup date.
        /// </value>
        [Display(Name = "Date")]
        [DataType(DataType.Date, ErrorMessage = "Please enter the date in format yyyy-mm-dd or mm/dd/yyyy")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LineupDate { get; set; }

        /// <summary>
        /// Gets or sets the vehicles.
        /// </summary>
        /// <value>
        /// A list of <see cref="VehicleSelectListItem"/> used to populate drop-down lists.
        /// </value>
        public List<VehicleSelectListItem> Vehicles { get; set; }

        /// <summary>
        /// Gets or sets the statuses.
        /// </summary>
        /// <value>
        /// The statuses.
        /// </value>
        public List<LineupMemberStatus> Statuses { get; set; }

        /// <summary>
        /// Gets or sets the shift commander.
        /// </summary>
        /// <value>
        /// The shift commander.
        /// </value>
        public LineupMember ShiftCommander { get; set; }

        /// <summary>
        /// Gets or sets the oic.
        /// </summary>
        /// <value>
        /// The oic.
        /// </value>
        public LineupMember OIC { get; set; }
        /// <summary>
        /// Gets or sets the members.
        /// </summary>
        /// <value>
        /// The members.
        /// </value>
        public List<LineupMember> Members { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is overlap.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is overlap; otherwise, <c>false</c>.
        /// </value>
        public bool IsOverlap { get; set; }

        /// <summary>
        /// Gets or sets the shift working.
        /// </summary>
        /// <value>
        /// The shift working.
        /// </value>
        public int ShiftWorking { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineupGeneratorViewModel"/> class.
        /// </summary>
        public LineupGeneratorViewModel()
        {
            Statuses = new List<LineupMemberStatus>()
            {
                new LineupMemberStatus(){ StatusId = 0, Status = "On Duty", IsOnDuty = true},
                new LineupMemberStatus(){ StatusId = 1, Status = "RDO", IsOnDuty = false},
                new LineupMemberStatus(){ StatusId = 2, Status = "TDY", IsOnDuty = false},
                new LineupMemberStatus(){ StatusId = 3, Status = "Training", IsOnDuty = false},
                new LineupMemberStatus(){ StatusId = 4, Status = "Leave (Annual)", IsOnDuty = false},
                new LineupMemberStatus(){ StatusId = 5, Status = "Leave (Comp)", IsOnDuty = false},
                new LineupMemberStatus(){ StatusId = 6, Status = "Leave (Military)", IsOnDuty = false},
                new LineupMemberStatus(){ StatusId = 7, Status = "Leave (FMLA)", IsOnDuty = false},
                new LineupMemberStatus(){ StatusId = 8, Status = "Leave (Other)", IsOnDuty = false},
                new LineupMemberStatus(){ StatusId = 9, Status = "Light Duty", IsOnDuty = true},
                new LineupMemberStatus(){ StatusId = 10, Status = "No Duty", IsOnDuty = false},
                new LineupMemberStatus(){ StatusId = 11, Status = "Admin Duty", IsOnDuty = true},
                new LineupMemberStatus(){ StatusId = 12, Status = "Desk", IsOnDuty = true},
                new LineupMemberStatus(){ StatusId = 13, Status = "On FTO", IsOnDuty = true},
                new LineupMemberStatus(){ StatusId = 15, Status = "Early Car", IsOnDuty = true},
                new LineupMemberStatus(){ StatusId = 16, Status = "Power Car", IsOnDuty = true},
            };
        }
    }
}