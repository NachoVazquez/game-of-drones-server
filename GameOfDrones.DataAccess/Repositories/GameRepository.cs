using System.Linq;
using System.Threading.Tasks;
using Dapper;
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

        public async Task<Game> GetGameWithPlayersByIdAsync(int id)
        {
            var queryGameWithRounds = $@"SELECT TOP(1) *
FROM Games
WHERE Id = @id;

SELECT *
FROM Rounds 
WHERE GameId = @id;";


            var connection = OpenConnection(out var closeManually);

            Game game;

            try
            {
                using (var queryResult = connection.QueryMultiple(queryGameWithRounds, new {@id = id}))
                {
                    game = await queryResult.ReadFirstOrDefaultAsync<Game>();

                    if (game != null)
                    {
                        game.Rounds = (await queryResult.ReadAsync<Round>()).ToList();
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            if (game == null) return null;

            var queryPlayer1 = $@"SELECT TOP(1) *
FROM Players
WHERE Id = {game.Player1Id}";

            var queryPlayer2 = $@"SELECT TOP(1) *
FROM Players
WHERE Id = {game.Player2Id}";

            game.Player1 = await QueryFirstOrDefaultAsync<Player>(queryPlayer1);
            game.Player2 = await QueryFirstOrDefaultAsync<Player>(queryPlayer2);

            return game;
        }
    }
}