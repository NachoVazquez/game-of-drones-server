using GameOfDrones.Core.Abstractions.DataAccess;
using GameOfDrones.Core.Domain.Models;
using GameOfDrones.DataAccess.Contexts;

namespace GameOfDrones.DataAccess.Repositories
{
    public class GameRepository : BaseRepository<Game, int>, IGameRepository
    {
        public GameRepository(GameOfDronesContext dbContext) : base(dbContext)
        {
        }
    }
}
