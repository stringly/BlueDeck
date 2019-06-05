using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    /// <summary>
    /// A Class that represents the <see cref="Gender"/> entity in a WebAPI request.
    /// </summary>
    public class GenderApiResult
    {
        /// <summary>
        /// Gets or sets the gender identifier.
        /// </summary>
        /// <value>
        /// The gender identifier.
        /// </value>
        public int GenderId {get;set;}

        /// <summary>
        /// Gets or sets the name of the gender.
        /// </summary>
        /// <value>
        /// The name of the gender.
        /// </value>
        public string GenderName { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation.
        /// </summary>
        /// <value>
        /// The abbreviation.
        /// </value>
        public string Abbreviation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenderApiResult"/> class.
        /// </summary>
        public GenderApiResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenderApiResult"/> class.
        /// </summary>
        /// <param name="_gender">The gender.</param>
        public GenderApiResult(Gender _gender)
        {
            GenderId = (Int32)_gender.GenderId;
            GenderName = _gender.GenderFullName;
            Abbreviation = _gender.Abbreviation.ToString();
        }
    }
}
