using System.Data.Entity;

namespace CricketDataIngester.Data
{
    public class PlayerContext : DbContext
    {
        public PlayerContext() : base("name=PlayerConnectionString")
        {
            
        }

        public DbSet<Player> Players { get; set; }
    }
}
