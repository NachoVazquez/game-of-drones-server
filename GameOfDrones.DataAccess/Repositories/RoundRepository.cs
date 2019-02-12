using GameOfDrones.Core.Abstractions.DataAccess;
using GameOfDrones.Core.Domain.Models;
using GameOfDrones.DataAccess.Contexts;

namespace GameOfDrones.DataAccess.Repositories
{
    public class RoundRepository : BaseRepository<Round, int>, IRoundRepository
    {
        public RoundRepository(GameOfDronesContext dbContext) : base(dbContext)
        {
        }
    }
}
