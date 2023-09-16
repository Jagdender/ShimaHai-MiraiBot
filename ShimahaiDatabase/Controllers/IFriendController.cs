using ShimahaiDatabase.Models;

namespace ShimahaiDatabase.Controllers
{
    public interface IFriendController
    {
        public Task<List<Friend>> GetFriends();
        public Task AddFriends(IEnumerable<Friend> friends);
        public Task<Friend?> GetFriend(int id);
        public Task<Friend?> GetFriend(
            string param,
            QueryBy query = QueryBy.None,
            bool isHC = false
        );
        public Task AddFriend(Friend friend);
    }

    public enum QueryBy
    {
        None,
        DefaultName,
        EnglishName,
        ChineseName,
        Nickname,
        SciName,
    }
}
