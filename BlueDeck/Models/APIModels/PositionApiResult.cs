using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    public class PositionApiResult
    {
        public int PositionId { get; set; }
        public string Name { get; set; }
        public bool IsManager { get; set; }
        public bool IsUnique { get; set; }
        public string Callsign { get; set; }
        public SubComponentApiResult Component { get; set; }
        public IEnumerable<SubMemberApiResult> Members { get; set; }

        public PositionApiResult()
        {
        }

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
