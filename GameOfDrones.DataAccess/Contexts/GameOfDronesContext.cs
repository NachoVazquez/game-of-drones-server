using GameOfDrones.Core.Abstractions.DataAccess;
using GameOfDrones.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GameOfDrones.DataAccess.Contexts
{
    public class GameOfDronesContext : DbContext, ISqlDbContext
    {
        public GameOfDronesContext(DbContextOptions<GameOfDronesContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);




            builder.Entity<Game>()
               .HasOne(g => g.Player1)
               .WithMany(p => p.GamesAsPlayer1)
               .HasForeignKey(g => g.Player1Id)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Game>()
               .HasOne(g => g.Player2)
               .WithMany(p => p.GamesAsPlayer2)
               .HasForeignKey(g => g.Player2Id)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Round>()
            .HasOne(r => r.Game)
            .WithMany(g => g.Rounds)
            .HasForeignKey(r => r.GameId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Round>()
               .Property(c => c.Player1Move)
               .HasConversion<int>();

            builder.Entity<Round>()
               .Property(c => c.Player2Move)
               .HasConversion<int>();

            builder.Entity<Round>()
               .Property(c => c.WinnerMove)
               .HasConversion<int>();

            builder.Entity<Player>()
              .HasIndex(p => p.UserName)
              .IsUnique();
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Round> Rounds { get; set; }
    }
}