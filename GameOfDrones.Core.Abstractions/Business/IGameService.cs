using GameOfDrones.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones.Core.Abstractions.Business
{
    public interface IGameService: IApplicationService<Game, int>
    {
        /// <summary>
        /// Creates a game for two players. If any of the players haven't play before they are added to the system
        /// </summary>
        /// <param name="player1Name">The name of the first player</param>
        /// <param name="player2Name">The name of the second player</param>
        /// <param name="roundsToWin">The amount rounds necessary to win the game</param>
        /// <returns></returns>
        Task<Game> CreateGameFromPlayerNamesAsync(string player1Name, string player2Name, int roundsToWin = 3);

        Task<Game> CreateRoundAsync(Round roundToCreate);

        Task<Game> GetGameWithPlayersByIdAsync(int id);


    }
}
