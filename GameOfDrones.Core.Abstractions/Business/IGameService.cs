using GameOfDrones.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones.Core.Abstractions.Business
{
    /// <summary>
    /// Business logic of the game
    /// </summary>
    public interface IGameService : IApplicationService<Game, int>
    {
        /// <summary>
        /// Creates a game for two players. If any of the players haven't play before they are added to the system
        /// </summary>
        /// <param name="player1Name">The name of the first player</param>
        /// <param name="player2Name">The name of the second player</param>
        /// <param name="roundsToWin">The amount rounds necessary to win the game</param>
        /// <returns></returns>
        Task<Game> CreateGameFromPlayerNamesAsync(string player1Name, string player2Name, int roundsToWin = 3);


        /// <summary>
        /// Creates a round for the game, refreshing players and game statistics
        /// </summary>
        /// <param name="roundToCreate">Round to be evaluate and created</param>
        /// <returns></returns>
        Task<Game> CreateRoundAsync(Round roundToCreate);

        /// <summary>
        /// Retrieve a game with its players and rounds given its Id
        /// </summary>
        /// <param name="id">The id of the game to be found</param>
        /// <returns></returns>
        Task<Game> GetGameWithPlayersByIdAsync(int id);
    }
}