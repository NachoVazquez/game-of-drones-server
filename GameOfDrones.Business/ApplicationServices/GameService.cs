using GameOfDrones.Business.ApplicationServices.Base;
using GameOfDrones.Core.Abstractions.Business;
using GameOfDrones.Core.Abstractions.DataAccess;
using GameOfDrones.Core.Domain.Models;
using System.Threading.Tasks;

namespace GameOfDrones.Business.ApplicationServices
{
    public class GameService : BaseApplicationService<Game, int>, IGameService
    {
        public IPlayerRepository PlayerRepository { get; set; }

        public GameService(IRepository<Game, int> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            this.PlayerRepository = UnitOfWork.PlayerRepository;
        }

        public async Task<Game> CreateGameFromPlayerNamesAsync(string player1Name, string player2Name, int roundsToWin = 3)
        {
            var player1 = await PlayerRepository.FindByUsernameAsync(player1Name);
            if (player1 == null)
            {
                player1 = await this.PlayerRepository.AddAsync(new Player { UserName = player1Name, GamesWon = 0, GamesLost = 0, RoundsLost = 0, RoundsWon = 0 });

                if (player1 == null)
                {
                    return null;
                }
            }

            var player2 = await PlayerRepository.FindByUsernameAsync(player2Name);
            if (player2 == null)
            {
                player2 = await this.PlayerRepository.AddAsync(new Player { UserName = player2Name, GamesWon = 0, GamesLost = 0, RoundsLost = 0, RoundsWon = 0 });

                if (player2 == null)
                {
                    return null;
                }
            }

            var gameToCreate = new Game { Player1Id = player1.Id, Player2Id = player2.Id, Player1RoundsWon = 0, Player2RoundsWon = 0, RoundsToWin = roundsToWin};

            var createdGame = await this.Repository.AddAsync(gameToCreate);

            return createdGame;
        }
    }


}
