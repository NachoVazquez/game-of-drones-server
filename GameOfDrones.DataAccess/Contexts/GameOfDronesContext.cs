using GameOfDrones.Core.Abstractions.DataAccess;
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
        }
    }
}