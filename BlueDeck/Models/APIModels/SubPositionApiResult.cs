using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    public class SubPositionApiResult
    {
        public int PositionId { get; set; }
        public string Name { get; set; }
        public bool IsManager { get; set; }
        public bool IsUnique { get; set; }
        public string Callsign { get; set; }

        public SubPositionApiResult()
        {
        }

        public SubPositionApiResult(Position _p)
        {
            PositionId = _p.PositionId;
            Name = _p.Name;
            IsManager = _p.IsManager;
            IsUnique = _p.IsUnique;
            Callsign = _p.Callsign;
        }


    }
}
