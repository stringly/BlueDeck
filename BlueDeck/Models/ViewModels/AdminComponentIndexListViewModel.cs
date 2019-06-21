﻿using BlueDeck.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.ViewModels
{
    /// <summary>
    /// Viewmodel used for the Component Entity used in the Admin/ComponentIndex View
    /// </summary>
    public class AdminComponentIndexListViewModel
    {
        /// <summary>
        /// Gets or sets the components.
        /// </summary>
        /// <value>
        /// The components.
        /// </value>
        public IEnumerable<AdminComponentIndexViewModelListItem> Components { get; set; }

        /// <summary>
        /// Gets or sets the paging information.
        /// </summary>
        /// <value>
        /// The paging information.
        /// </value>
        public PagingInfo PagingInfo { get; set; }

        /// <summary>
        /// Gets or sets the name sort.
        /// </summary>
        /// <value>
        /// The name sort.
        /// </value>
        public string NameSort { get; set; }

        /// <summary>
        /// Gets or sets the parent component name sort.
        /// </summary>
        /// <value>
        /// The parent component name sort.
        /// </value>
        public string ParentComponentNameSort { get; set; }

        /// <summary>
        /// Gets or sets the current filter.
        /// </summary>
        /// <value>
        /// The current filter.
        /// </value>
        public string CurrentFilter { get; set; }

        /// <summary>
        /// Gets or sets the current sort.
        /// </summary>
        /// <value>
        /// The current sort.
        /// </value>
        public string CurrentSort { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminComponentIndexListViewModel"/> class.
        /// </summary>
        public AdminComponentIndexListViewModel()
        {
            Components = new List<AdminComponentIndexViewModelListItem>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminComponentIndexListViewModel"/> class.
        /// </summary>
        /// <param name="_components">The components.</param>
        public AdminComponentIndexListViewModel(List<Component> _components)
        {
            Components = _components.ConvertAll(x => new AdminComponentIndexViewModelListItem(x));
        }
    }
}
