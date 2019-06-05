using System.Collections.Generic;
using System.Linq;

namespace BlueDeck.Models.APIModels
{
    /// <summary>
    /// Class that represents a <see cref="Component"/> entity in a WebAPI request.
    /// </summary>
    public class ComponentApiResult
    {
        /// <summary>
        /// Gets or sets the component identifier.
        /// </summary>
        /// <value>
        /// The component identifier.
        /// </value>
        public int? ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the acronym.
        /// </summary>
        /// <value>
        /// The acronym.
        /// </value>
        public string Acronym { get; set; }

        /// <summary>
        /// Gets or sets the parent component.
        /// </summary>
        /// <remarks>
        /// This property is a <see cref="SubComponentApiResult"/> object.
        /// </remarks>
        /// <value>
        /// The parent component.
        /// </value>
        public SubComponentApiResult ParentComponent { get; set; }

        /// <summary>
        /// Gets or sets the positions.
        /// </summary>
        /// <remarks>
        /// This property is a <see cref="List{PositionApiResult}"/>
        /// </remarks>
        /// <value>
        /// The positions.
        /// </value>
        public List<PositionApiResult> Positions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentApiResult"/> class.
        /// </summary>
        public ComponentApiResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentApiResult"/> class.
        /// </summary>
        /// <param name="_component">The <see cref="Component"/> class object from which the <see cref="ComponentApiResult"/> object will be constructed.</param>
        public ComponentApiResult(Component _component)
        {
            ComponentId = _component?.ComponentId;
            Name = _component?.Name ?? "";
            Acronym = _component?.Acronym ?? "";
            if (_component?.ParentComponent != null)
            {
                ParentComponent = new SubComponentApiResult(_component.ParentComponent);                
            }
            if (_component?.Positions != null)
            {
                Positions = _component.Positions.ToList().ConvertAll(x => new PositionApiResult(x));
            }
            
        }
    }
}
