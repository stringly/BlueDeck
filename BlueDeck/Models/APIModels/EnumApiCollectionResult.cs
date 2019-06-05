using System.Collections.Generic;

namespace BlueDeck.Models.APIModels
{
    /// <summary>
    /// A Class that represents a collection of results for all BlueDeck enumerated types in a WebAPI request.
    /// </summary>
    public class EnumApiCollectionResult
    {
        /// <summary>
        /// Gets or sets the genders.
        /// </summary>
        /// <value>
        /// A collection of <see cref="GenderApiResult"/>
        /// </value>
        public IEnumerable<GenderApiResult> Genders { get; set; }

        /// <summary>
        /// Gets or sets the ranks.
        /// </summary>
        /// <value>
        /// A collection of <see cref="RankApiResult"/>
        /// </value>
        public IEnumerable<RankApiResult> Ranks { get; set; }

        /// <summary>
        /// Gets or sets the races.
        /// </summary>
        /// <value>
        /// A collection of <see cref="RaceApiResult"/>
        /// </value>
        public IEnumerable<RaceApiResult> Races { get; set; }

        /// <summary>
        /// Gets or sets the duty statuses.
        /// </summary>
        /// <value>
        /// A collection of <see cref="DutyStatusApiResult"/>
        /// </value>
        public IEnumerable<DutyStatusApiResult> DutyStatuses { get; set; }

        /// <summary>
        /// Gets or sets the role types.
        /// </summary>
        /// <value>
        /// A collection of <see cref="RoleTypeApiResult"/>
        /// </value>
        public IEnumerable<RoleTypeApiResult> RoleTypes { get; set; }

        /// <summary>
        /// Gets or sets the phone types.
        /// </summary>
        /// <value>
        /// A collection of <see cref="PhoneNumberTypeApiResult"/>
        /// </value>
        public IEnumerable<PhoneNumberTypeApiResult> PhoneTypes { get; set; }

        /// <summary>
        /// Gets or sets the application statuses.
        /// </summary>
        /// <value>
        /// A collection of <see cref="AppStatusApiResult"/>
        /// </value>
        public IEnumerable<AppStatusApiResult> AppStatuses { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumApiCollectionResult"/> class.
        /// </summary>
        public EnumApiCollectionResult()
        {
            Genders = new List<GenderApiResult>();
            Ranks = new List<RankApiResult>();
            Races = new List<RaceApiResult>();
            DutyStatuses = new List<DutyStatusApiResult>();
            RoleTypes = new List<RoleTypeApiResult>();
            PhoneTypes = new List<PhoneNumberTypeApiResult>();
            AppStatuses = new List<AppStatusApiResult>();
        }
    }
}
