namespace ShimahaiDatabase.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameFlag { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string NameSci { get; set; } = string.Empty;
        public string NameCn { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string Introduction { get; set; } = string.Empty;
        public string Greeting { get; set; } = string.Empty;

        public FriendData FriendData { get; set; } = null!;
    }
}
