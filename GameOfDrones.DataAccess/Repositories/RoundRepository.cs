using System.Threading.Tasks;
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

        public async Task<long> GetRoundsPlayed()
        {
            var query = $@"SELECT COUNT(Id)
FROM Rounds";

            return await this.QueryFirstOrDefaultAsync<long>(query);
        }
    }
}