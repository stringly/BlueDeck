using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Types
{
    public class ApplicationStatusSelectListItem
    {
        public int? Id {get;set;}
        public string Name {get;set;}

        public ApplicationStatusSelectListItem()
        {
        }

        public ApplicationStatusSelectListItem(AppStatus _status)
        {
            Id = _status.AppStatusId;
            Name = _status.StatusName;
        }
    }
}
