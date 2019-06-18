using BlueDeck.Models.Enums;

namespace BlueDeck.Models.APIModels
{
    /// <summary>
    /// A Class that represents a <see cref="DutyStatus"/> entity in a WebAPI response.
    /// </summary>
    public class DutyStatusApiResult
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation.
        /// </summary>
        /// <value>
        /// The abbreviation.
        /// </value>
        public string Abbreviation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has police power.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has police power; otherwise, <c>false</c>.
        /// </value>
        public bool HasPolicePower { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DutyStatusApiResult"/> class.
        /// </summary>
        public DutyStatusApiResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DutyStatusApiResult"/> class.
        /// </summary>
        /// <param name="_dutyStatus">The duty status.</param>
        public DutyStatusApiResult(DutyStatus _dutyStatus)
        {
            Name = _dutyStatus.DutyStatusName;
            Abbreviation = _dutyStatus.Abbreviation.ToString();
            HasPolicePower = _dutyStatus.HasPolicePower;
        }
    }
}
