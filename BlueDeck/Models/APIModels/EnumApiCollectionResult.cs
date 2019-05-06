using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    public class EnumApiCollectionResult
    {
        public IEnumerable<Gender> Genders { get; set; }
        public IEnumerable<Rank> Ranks { get; set; }
        public IEnumerable<Race> Races { get; set; }
        public IEnumerable<DutyStatus> DutyStatuses { get; set; }
        public IEnumerable<RoleType> RoleTypes { get; set; }
        public IEnumerable<PhoneNumberType> PhoneTypes { get; set; }
        public IEnumerable<AppStatus> AppStatuses { get; set; }

        public EnumApiCollectionResult()
        {
            Genders = new List<Gender>();
            Ranks = new List<Rank>();
            Races = new List<Race>();
            DutyStatuses = new List<DutyStatus>();
            RoleTypes = new List<RoleType>();
            PhoneTypes = new List<PhoneNumberType>();
            AppStatuses = new List<AppStatus>();
        }
    }
}
