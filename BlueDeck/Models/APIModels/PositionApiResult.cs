using System.Collections.Generic;
using System.Linq;

namespace BlueDeck.Models.APIModels
{
    /// <summary>
    /// Class that represents a <see cref="Position"/> entity in a WebAPI response.
    /// </summary>
    public class PositionApiResult
    {
        /// <summary>
        /// Gets or sets the position identifier.
        /// </summary>
        /// <value>
        /// The position identifier, derived from <see cref="Position.PositionId"/>.
        /// </value>
        public int PositionId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name, derived from <see cref="Position.Name"/>
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is manager.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is manager; otherwise, <c>false</c>.
        /// </value>
        public bool IsManager { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is unique.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is unique; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnique { get; set; }

        /// <summary>
        /// Gets or sets the callsign.
        /// </summary>
        /// <value>
        /// The callsign.
        /// </value>
        public string Callsign { get; set; }

        /// <summary>
        /// Gets or sets the Parent Component of this Position.
        /// </summary>
        /// <value>
        /// The component, which is a <see cref="SubComponentApiResult"/> derived from <see cref="Position.ParentComponent"/>
        /// </value>
        public SubComponentApiResult Component { get; set; }
        /// <summary>
        /// Gets or sets the members assigned to this Position.
        /// </summary>
        /// <value>
        /// The members, which is a list of <see cref="SubMemberApiResult"/> objects derived from the <see cref="Position.Members"/>
        /// </value>
        public IEnumerable<SubMemberApiResult> Members { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionApiResult"/> class.
        /// <remarks>
        /// Default, parameterless constructor.
        /// </remarks>
        /// </summary>
        public PositionApiResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionApiResult"/> class.
        /// </summary>
        /// <remarks>
        /// Constructor that accepts a <see cref="Position"/> parameter.
        /// </remarks>
        /// <param name="_position">A <see cref="Position"/>.</param>
        public PositionApiResult(Position _position)
        {
            PositionId = _position.PositionId;
            Name = _position.Name;
            IsManager = _position.IsManager;
            IsUnique = _position.IsUnique;
            Callsign = _position.Callsign;
            Component = new SubComponentApiResult(_position.ParentComponent);
            Members = _position.Members.ToList().ConvertAll(x => new SubMemberApiResult(x));
        }
    }
}
