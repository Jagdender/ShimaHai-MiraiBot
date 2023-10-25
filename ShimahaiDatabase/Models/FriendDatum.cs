namespace ShimahaiDatabase.Models
{
    public class FriendDatum
    {
        public int Id { get; set; }
        public int Rarity { get; set; }
        public Attri Attribute { get; set; }
        public int Status { get; set; }
        public int Hp { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public double Evd { get; set; }
    }

    public enum Attri
    {
        NONE,
        Funny = 1,
        Friendly,
        Relax,
        Lovely,
        Active,
        MyPace,
        Funny_Friendly = 12,
        Funny_Relax,
        Funny_Lovely,
        Funny_Active,
        Funny_MyPace,
        Friendly_Funny = 21,
        Friendly_Relax,
        Friendly_Lovely,
        Friendly_Active,
        Friendly_MyPace,
        Relax_Funny = 31,
        Relax_Friendly,
        Relax_Lovely,
        Relax_Active,
        Relax_MyPace,
        Lovely_Funny = 41,
        Lovely_Friendly,
        Lovely_Relax,
        Lovely_Active,
        Lovely_MyPace,
        Active_Funny = 51,
        Active_Friendly,
        Active_Relax,
        Active_Lovely,
        Active_MyPace,
        MyPace_Funny = 61,
        MyPace_Friendly,
        MyPace_Relax,
        MyPace_Lovely,
        MyPace_Active,
    }
}
