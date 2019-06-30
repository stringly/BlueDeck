using BlueDeck.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.ViewModels
{
    /// <summary>
    /// View Model used in the Documents/Index View
    /// </summary>
    public class DocumentsIndexViewModel
    {
        /// <summary>
        /// Gets or sets the components.
        /// </summary>
        /// <value>
        /// A list of <see cref="ComponentSelectListItem"/> used to populate select lists.
        /// </value>
        public List<ComponentSelectListItem> Components { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsIndexViewModel"/> class.
        /// </summary>
        /// <param name="_components">A list of <see cref="ComponentSelectListItem"/>.</param>
        public DocumentsIndexViewModel(List<ComponentSelectListItem> _components)
        {
            Components = _components;
        }
    }
}
