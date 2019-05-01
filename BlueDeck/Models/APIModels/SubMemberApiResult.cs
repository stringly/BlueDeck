using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    public class SubMemberApiResult
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PGPDId { get; set; }
        public string Email { get; set; }
        public IEnumerable<ContactNumberApiResult> ContactNumbers { get; set; }
        public RankApiResult Rank { get; set; }
        public GenderApiResult Gender { get; set; }
        public RaceApiResult Race { get; set; }
        public DutyStatusApiResult DutyStatus { get; set; }

        public SubMemberApiResult()
        {
        }

        public SubMemberApiResult(Member _m)
        {
            MemberId = _m.MemberId;
            FirstName = _m.FirstName;
            LastName = _m.LastName;
            PGPDId = _m.IdNumber;
            Email = _m.Email;
            ContactNumbers = _m.PhoneNumbers.ToList().ConvertAll(x => new ContactNumberApiResult(x));
            Rank = new RankApiResult(_m.Rank);
            Gender = new GenderApiResult(_m.Gender);
            Race = new RaceApiResult(_m.Race);
            DutyStatus = new DutyStatusApiResult(_m.DutyStatus);
        }
    }
}
