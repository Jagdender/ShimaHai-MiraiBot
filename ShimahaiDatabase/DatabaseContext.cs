using Microsoft.EntityFrameworkCore;
using ShimahaiDatabase.Models;

namespace ShimahaiDatabase
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }

        public DbSet<Friend> Friends { get; set; }
        public DbSet<FriendData> FriendData { get; set; }
    }
}
