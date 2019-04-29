using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    public class GenderApiResult
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public GenderApiResult()
        {
        }

        public GenderApiResult(Gender _gender)
        {
            Name = _gender.GenderFullName;
            Abbreviation = _gender.Abbreviation.ToString();
        }
    }
}
