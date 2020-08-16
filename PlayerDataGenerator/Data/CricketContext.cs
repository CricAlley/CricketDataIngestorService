using Microsoft.EntityFrameworkCore;

namespace PlayerDataGenerator.Data
{
    public class CricketContext : DbContext
    {
        public CricketContext(DbContextOptions<CricketContext> options) : base(options)
        {
            
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerAliasMapping> PlayerAliasMapping { get; set; }
        public DbSet<ExcludedTeam> ExcludedTeams { get; set; }
    }
}
