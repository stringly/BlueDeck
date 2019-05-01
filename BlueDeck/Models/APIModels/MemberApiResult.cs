using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    public class MemberApiResult
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
        public SubPositionApiResult Position { get; set; }
        public SubMemberApiResult Supervisor { get; set; }

        public MemberApiResult()
        {
        }
        
        public MemberApiResult(Member _member)
        {
            MemberId = _member.MemberId;
            FirstName = _member.FirstName;
            LastName = _member.LastName;
            PGPDId = _member.IdNumber;
            Email = _member.Email;
            ContactNumbers = _member.PhoneNumbers.ToList().ConvertAll(x => new ContactNumberApiResult(x));
            Rank = new RankApiResult(_member.Rank);
            Gender = new GenderApiResult(_member.Gender);
            Race = new RaceApiResult(_member.Race);
            DutyStatus = new DutyStatusApiResult(_member.DutyStatus);
            Position = new SubPositionApiResult(_member.Position);            
        }
    }
}
