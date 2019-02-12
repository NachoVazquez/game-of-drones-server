using GameOfDrones.Core.Abstractions.DataAccess;
using GameOfDrones.Core.Domain.Models;
using GameOfDrones.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GameOfDrones.DataAccess.Repositories
{
    public class PlayerRepository : BaseRepository<Player, int>, IPlayerRepository
    {
        public PlayerRepository(GameOfDronesContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> ExistByUsernameAsync(string userName) =>
            await this.Entities.AnyAsync(ent => ent.UserName.Equals(userName));

        public async Task<Player> FindByUsernameAsync(string userName)
        {
            var query = $@"SELECT *
FROM Players
WHERE UserName = '{userName}'";
            return await QueryFirstOrDefaultAsync<Player>(query);
        }
    }
}