using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    public class RoleTypeApiResult
    {
        public int RoleTypeId { get; set; }
        public string RoleTypeName { get; set; }

        public RoleTypeApiResult(RoleType _roleType)
        {
            RoleTypeId = _roleType.RoleTypeId;
            RoleTypeName = _roleType.RoleTypeName;
        }
    }
}
