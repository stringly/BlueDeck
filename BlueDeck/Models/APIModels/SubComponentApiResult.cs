using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    public class SubComponentApiResult
    {
        public int? ComponentId { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }

        public SubComponentApiResult()
        {
        }

        public SubComponentApiResult(Component _c)
        {
            ComponentId = _c.ComponentId;
            Name = _c.Name;
            Acronym = _c.Acronym;
        }
    }
}
