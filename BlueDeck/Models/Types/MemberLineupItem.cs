namespace BlueDeck.Models.Types
{
    public class MemberLineupItem
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberRankImgSource { get; set; }
        public char MemberGender { get; set; }
        public char MemberRace { get; set; }

        public MemberLineupItem()
        {
        }

        public MemberLineupItem(Member _m)
        {
            MemberId = _m.MemberId;
            MemberName = _m.GetTitleName();
            MemberRankImgSource = _m.Rank.GetRankImageSource();
            MemberGender = _m.Gender.Abbreviation;
            MemberRace = _m.Race.Abbreviation;
        }
    }
}
