using Microsoft.EntityFrameworkCore;
using ShimahaiDatabase.Models;

namespace ShimahaiDatabase.Controllers
{
    public class FriendController : IFriendController
    {
        private readonly DatabaseContext _databaseContext;

        public FriendController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<List<Friend>> GetFriends()
        {
            return _databaseContext.Friends.ToListAsync();
        }

        public Task AddFriends(IEnumerable<Friend> friends)
        {
            _databaseContext.AddRangeAsync(friends);
            return _databaseContext.SaveChangesAsync();
        }

        public Task AddFriend(Friend friend)
        {
            _databaseContext.AddAsync(friend);
            return _databaseContext.SaveChangesAsync();
        }

        public Task<Friend?> GetFriend(int id)
        {
            return _databaseContext.Friends.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Friend?> GetFriend(
            string param,
            QueryBy query = QueryBy.None,
            bool isHC = false
        )
        {
            Friend? friend = null!;
            bool isNone = query == QueryBy.None;

            var expression = _databaseContext.Friends.Where(
                f =>
                    (isHC && f.NameFlag != string.Empty)
                    || (!isHC && f.NameFlag.ToLower() == string.Empty)
            );
            if (query == QueryBy.ChineseName || isNone)
            {
                friend = await expression.FirstOrDefaultAsync(f => f.NameCn.ToLower() == param);
                if ((friend is null && !isNone) || friend is not null)
                    return friend;
            }
            if (query == QueryBy.DefaultName || isNone)
            {
                friend = await expression.FirstOrDefaultAsync(x => x.Name == param);
                if ((friend is null && !isNone) || friend is not null)
                    return friend;
            }
            if (query == QueryBy.EnglishName || isNone)
            {
                friend = await expression.FirstOrDefaultAsync(f => f.NameEn.ToLower() == param);
                if ((friend is null && !isNone) || friend is not null)
                    return friend;
            }
            if (query == QueryBy.SciName || isNone)
            {
                friend = await expression.FirstOrDefaultAsync(f => f.NameSci.ToLower() == param);
                if ((friend is null && !isNone) || friend is not null)
                    return friend;
            }
            if (query == QueryBy.Nickname || isNone)
            {
                friend = await expression.FirstOrDefaultAsync(f => f.Nickname.Contains(param));
                if ((friend is null && !isNone) || friend is not null)
                    return friend;
            }

            return null;
        }
    }
}
